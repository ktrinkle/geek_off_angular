import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Subject } from 'rxjs';
import { Location } from '@angular/common';
import { Store } from '@ngrx/store';
import { currentEvent } from './store/eventManage/eventManage.actions';
import { AuthService } from './service/auth.service';
import { PlayerGuard } from './player.guard';


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
    '/control/round2feud',
    '/control/round1',
    '/control/round3',
    '/home',
    '/',
  ]
  private readonly _destroying$ = new Subject<void>();
  public profile: ProfileType | undefined;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private authService: AuthService,
    private playerGuard: PlayerGuard,
    private router: Router,
    private location: Location,
    private store: Store) {
    router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.showLoginBar = this.pagesToShowLogin.indexOf(event.url) > -1;
      }
    });
    this.store.dispatch(currentEvent());
    console.log('currentevent finished');
  }

  ngOnInit(): void {
    // const currentPath = this.location.path();
    this.authService.loggedIn$?.subscribe(l => {
      this.loginDisplay = l;
    });
  }

  logout() {
    this.authService.logout();
  }

  ngOnDestroy(): void {
    this._destroying$.next(undefined);
    this._destroying$.complete();
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
