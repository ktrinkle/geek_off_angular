import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
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
    this.setLoginDisplay();
  }

  async login() {

  }

  logout() { // Add log out function here
  }

  setLoginDisplay() {
    console.log('setLoginDisplay');
      this.loginDisplay = false;
  }

  ngOnDestroy(): void {
    this._destroying$.next(undefined);
    this._destroying$.complete();
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
