import { Injectable } from '@angular/core';
import { adminLogin, simpleUser, teamLogin } from '../data/data';
import { DataService } from '../data.service';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private currentUserSubject : BehaviorSubject<simpleUser>;
  private currentUser : Observable<simpleUser>;
  private currentTokenSubject: BehaviorSubject<string>;
  private currentToken: Observable<string>;

  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private dataService:DataService)
  {
    this.currentUserSubject = new BehaviorSubject<simpleUser>({
      teamNum: localStorage.getItem('teamNum') === '0' ? undefined : Number(localStorage.getItem("teamNum")),
      teamName: localStorage.getItem('teamName') ?? undefined
    });

    this.currentUser = this.currentUserSubject.asObservable();

    this.currentTokenSubject = new BehaviorSubject<any>(sessionStorage.getItem('jwt'));
    this.currentToken = this.currentTokenSubject.asObservable();


    if(localStorage.getItem('teamNum') != null)
    {
      this.loggedIn.next(true);
    }
  }

  public get currentUserValue(): simpleUser{
    return this.currentUserSubject.value;
  }

  public getAccessToken(): Observable<string> {
    return this.currentToken;
  }

  public processLoginAdmin(loginDto: adminLogin): void
  {
    if (loginDto.userName != '' && loginDto.password != '')
    {
      this.dataService.sendAdminLogin(loginDto).subscribe(al =>
        {
          localStorage.setItem('jwt', al.bearerToken);
          localStorage.setItem('teamNum', '0');
          localStorage.setItem('teamName', al.humanName);
        })

    }
  }

  public processLoginTeam(loginDto: teamLogin): boolean
  {
    var rtn: boolean = false;
    if (loginDto.yEvent != '' && loginDto.teamGuid !== null)
    {
      this.dataService.sendTeamLogin(loginDto).subscribe(al =>
        {
          if (al.bearerToken != null)
          {
            localStorage.setItem('jwt', al.bearerToken);
            localStorage.setItem('teamNum', al.teamNum.toString());
            localStorage.setItem('teamName', al.humanName);
            rtn = true;
          }
        })
    }

    return rtn;
  }

  public get isUserLoggedIn(){
    return this.loggedIn.asObservable();
  }

}
