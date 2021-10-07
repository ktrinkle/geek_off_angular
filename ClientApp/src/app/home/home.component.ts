import { Component, OnInit } from '@angular/core';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { EventMessage, EventType, InteractionStatus } from '@azure/msal-browser';
import { filter } from 'rxjs/operators';
import { PlayerGuard } from '../player.guard';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  userIsLoggedIn: boolean = false;
  userIsAdmin: boolean = false;

  constructor(
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private playerGuard: PlayerGuard) { }

  ngOnInit(): void {
    // authentication handling
    this.msalBroadcastService.msalSubject$
    .pipe(
      filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS),
    )
    .subscribe((result: EventMessage) => {
      console.log(result);
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
    if (this.userIsLoggedIn)
    {
      this.userIsAdmin = this.playerGuard.checkAdmin("admin");
    }
    console.log(this.userIsLoggedIn);
  }

}
