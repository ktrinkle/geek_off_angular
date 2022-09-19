import { Component, OnInit } from '@angular/core';
import { filter, takeUntil } from 'rxjs/operators';
import { PlayerGuard } from '../player.guard';
import { Store } from '@ngrx/store';
import { selectCurrentEvent } from '../store';
import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  userIsLoggedIn: boolean = false;
  userIsAdmin: boolean = false;
  public yEvent: string = '';
  destroy$: Subject<boolean> = new Subject<boolean>();


  constructor(
    private playerGuard: PlayerGuard,
    private jwtHelper : JwtHelperService,
    private store: Store) { }

  ngOnInit(): void {
    // authentication handling

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
    });

  }

  setHeaderDisplay() {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.userIsLoggedIn = true;
      this.userIsAdmin = this.playerGuard.checkRole("admin");
    } else {
      this.userIsLoggedIn = false;
      this.userIsAdmin = false;
    }
    console.log(this.userIsLoggedIn);
  }


}
