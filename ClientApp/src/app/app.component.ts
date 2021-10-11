import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';
import { MsalService, MsalBroadcastService, MSAL_GUARD_CONFIG, MsalGuardConfiguration } from '@azure/msal-angular';
import { AuthenticationResult, InteractionStatus, InteractionType, PopupRequest, RedirectRequest } from '@azure/msal-browser';
import { Subject, BehaviorSubject, Observable } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { DataService } from './data.service';

type ProfileType = {
  playerName: string,
  userName: string,
  teamNum: number,
  playerNum: number,
  admin: boolean
};

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Geek Off';
  isIframe = false;
  loginDisplay = false;
  showLoginBar = true;
  pagesToShowLogin = [
    '/round1/contestant',
    '/control/pregame',
    '/control/round2',
    '/control/round1',
    '/home',
    '/',
  ]
  private currentEventSubject: BehaviorSubject<string>;
  public currentEvent: Observable<string>;
  private readonly _destroying$ = new Subject<void>();
  public profile:ProfileType | undefined;

  constructor(
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private dataService: DataService,
    private router: Router) {
      router.events.forEach((event) => {
        if (event instanceof NavigationEnd) {
          console.log(event.url);
          console.log(this.pagesToShowLogin.indexOf(event.url));
          this.showLoginBar = this.pagesToShowLogin.indexOf(event.url) > -1;
        }
      });
      this.dataService.getCurrentEvent().subscribe(event => {
        sessionStorage.setItem('event', event);
      });

      this.currentEventSubject = new BehaviorSubject<any>(sessionStorage.getItem('event'));
      this.currentEvent = this.currentEventSubject.asObservable();

    }

  ngOnInit(): void {
    this.isIframe = window !== window.parent && !window.opener;

    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        takeUntil(this._destroying$)
      )
      .subscribe(() => {
        this.setLoginDisplay();
      });
  }

  login() {
    if (this.msalGuardConfig.authRequest){
      this.authService.loginRedirect({...this.msalGuardConfig.authRequest} as RedirectRequest);
    } else {
      this.authService.loginRedirect();
    }
  }

  logout() { // Add log out function here
    this.authService.logout();
  }

  setLoginDisplay() {
    if (this.authService.instance.getAllAccounts().length > 0)
    {
      this.getAdInfo();
      this.loginDisplay = true;
    }
    else{
      this.loginDisplay = false;
    }
  }

  getAdInfo() {
    this.dataService.getADProfile().subscribe(ad => {
      this.profile = ad;
      console.log(this.profile);
    });
  }

  public get currentUserValue(): string {
    this.currentEventSubject.next(sessionStorage.getItem('event') ?? '');
    return this.currentEventSubject.value;
}

  ngOnDestroy(): void {
    sessionStorage.removeItem('event');
    this.currentEventSubject.next('');
    this._destroying$.next(undefined);
    this._destroying$.complete();
  }
}
