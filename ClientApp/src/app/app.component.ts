import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { MsalService, MsalBroadcastService, MSAL_GUARD_CONFIG, MsalGuardConfiguration } from '@azure/msal-angular';
import { BrowserUtils, InteractionStatus, RedirectRequest } from '@azure/msal-browser';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { DataService } from './data.service';
import { Location } from '@angular/common';
import { Store } from '@ngrx/store';
import { currentEvent } from './store/round1/round1.actions';
import { selectCurrentEvent } from './store';


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
export class AppComponent implements OnInit, OnDestroy {
  title = 'Geek Off';
  isIframe = false;
  public loginDisplay = false;
  showLoginBar = true;
  pagesToShowLogin = [
    '/round1/contestant',
    '/control/pregame',
    '/control/round2',
    '/control/round1',
    '/home',
    '/',
  ]
  private readonly _destroying$ = new Subject<void>();
  public profile: ProfileType | undefined;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private dataService: DataService,
    private router: Router,
    private location: Location,
    private store: Store) {
    router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        console.log(event.url);
        console.log(this.pagesToShowLogin.indexOf(event.url));
        this.showLoginBar = this.pagesToShowLogin.indexOf(event.url) > -1;
      }
    });
    this.store.dispatch(currentEvent());
  }

  ngOnInit(): void {
    const currentPath = this.location.path();
    this.isIframe = BrowserUtils.isInIframe() && !window.opener && currentPath.indexOf("logout") < 0;
    this.setLoginDisplay();

    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        takeUntil(this._destroying$)
      )
      .subscribe(() => {
        console.log('Broadcast service in progress');
        this.setLoginDisplay();
        this.checkAndSetActiveAccount();
      });
  }

  async login() {
    await this.authService.instance.handleRedirectPromise();

    if (this.msalGuardConfig.authRequest) {
      console.log('loginRedirect with wait');
      this.authService.loginRedirect({ ...this.msalGuardConfig.authRequest } as RedirectRequest);
    } else {
      console.log('loginRedirect null');
      this.authService.loginRedirect();
    }
  }

  logout() { // Add log out function here
    this.authService.logout();
  }

  setLoginDisplay() {
    console.log('setLoginDisplay');

    if (this.authService.instance.getAllAccounts().length > 0) {
      this.getAdInfo();
      this.loginDisplay = true;
    }
    else {
      this.loginDisplay = false;
    }
  }

  checkAndSetActiveAccount() {
    /**
     * If no active account set but there are accounts signed in, sets first account to active account
     * To use active account set here, subscribe to inProgress$ first in your component
     * Note: Basic usage demonstrated. Your app may require more complicated account selection logic
     */
    let activeAccount = this.authService.instance.getActiveAccount();

    if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
      let accounts = this.authService.instance.getAllAccounts();
      this.authService.instance.setActiveAccount(accounts[0]);
    }
  }

  getAdInfo() {
    this.dataService.getADProfile().subscribe(ad => {
      this.profile = ad;
      console.log(this.profile);
    });
  }

  ngOnDestroy(): void {
    this._destroying$.next(undefined);
    this._destroying$.complete();
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
