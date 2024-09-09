import { Injectable } from '@angular/core';
import { adminLogin, simpleUser, teamLogin } from '../data/data';
import { DataService } from '../data.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private currentUserSubject : BehaviorSubject<simpleUser>;
  private currentUser : Observable<simpleUser>;
  private currentTokenSubject: BehaviorSubject<string>;
  private currentToken: Observable<string>;

  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public loggedIn$: Observable<boolean>;

  private isAdmin: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public isAdmin$: Observable<boolean>;

  private expirationTimer: any;

  constructor(private dataService:DataService, private jwtHelper: JwtHelperService)
  {
    this.currentUserSubject = new BehaviorSubject<simpleUser>({
      teamNum: localStorage.getItem('teamNum') === '0' ? undefined : Number(localStorage.getItem("teamNum")),
      teamName: localStorage.getItem('teamName') ?? undefined
    });

    this.currentUser = this.currentUserSubject.asObservable();
    this.loggedIn$ = this.loggedIn.asObservable();
    this.isAdmin$ = this.isAdmin.asObservable();

    this.currentTokenSubject = new BehaviorSubject<any>(sessionStorage.getItem('jwt'));
    this.currentToken = this.currentTokenSubject.asObservable();

    if(localStorage.getItem('teamNum') != null) {
      this.loggedIn.next(true);
    }

    if(localStorage.getItem('teamNum') === "0") {
      this.isAdmin.next(true);
    }
  }

  public get currentUserValue(): simpleUser{
    return this.currentUserSubject.value;
  }

  public getAccessToken(): Observable<string> {
    return this.currentToken;
  }

  public processLoginAdmin(loginDto: adminLogin): boolean
  {
    var rtn: boolean = false;
    if (loginDto.userLogin.userName != '' && loginDto.userLogin.password != '')
    {
      this.dataService.sendAdminLogin(loginDto).subscribe(al =>
        {
          localStorage.setItem('jwt', al.bearerToken);
          localStorage.setItem('teamNum', '0');
          localStorage.setItem('teamName', al.humanName);

          if (al.bearerToken != null) {
            rtn = true;
            this.loggedIn.next(true);
            this.isAdmin.next(true);
            this.setExpirationTimer();
          }
        })
    }
    return rtn;
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
            this.loggedIn.next(true);
            this.isAdmin.next(false);
            this.setExpirationTimer();
          }
        })
    }

    return rtn;
  }

  public logout() {
    localStorage.removeItem('jwt');
    localStorage.removeItem('teamNum');
    localStorage.removeItem('teamName');
    this.loggedIn.next(false);
    this.isAdmin.next(false);
  }

  private setExpirationTimer() {
    this.expirationTimer = setInterval(() => { this.checkExpiration(); }, 14400000);

  }

  private checkExpiration(): void {
    const token = localStorage.getItem('jwt');
    if (this.jwtHelper.isTokenExpired(token)) {
      this.logout();
    }
  }

}
