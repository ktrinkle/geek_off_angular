import { Component, OnInit } from '@angular/core';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { AuthenticationResult, EventMessage, EventType, InteractionStatus } from '@azure/msal-browser';
import { filter, takeUntil } from 'rxjs/operators';
import { PlayerGuard } from '../player.guard';
import { Store } from '@ngrx/store';
import { selectCurrentEvent } from '../store';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  userIsLoggedIn: boolean = false;
  userIsAdmin: boolean = false;
  isIframe = false;
  public yEvent: string = '';
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private playerGuard: PlayerGuard,
    private store: Store) { }

  ngOnInit(): void {
    // authentication handling

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
    });

    this.isIframe = window !== window.parent && !window.opener;
    this.msalBroadcastService.msalSubject$
      .pipe(
        filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS),
      )
      .subscribe((result: EventMessage) => {
        console.log(result);
        const payload = result.payload as AuthenticationResult;
        this.authService.instance.setActiveAccount(payload.account);
      });

    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None)
      )
      .subscribe(() => {
        this.setHeaderDisplay();
      })
  }

  setHeaderDisplay() {
    this.userIsLoggedIn = this.authService.instance.getAllAccounts().length > 0;
    if (this.userIsLoggedIn) {
      this.userIsAdmin = this.playerGuard.checkAdmin("admin");
    }
    console.log(this.userIsLoggedIn);
  }

}
