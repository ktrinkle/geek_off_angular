import { Component, OnInit } from '@angular/core';
import { filter, takeUntil } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { selectCurrentEvent } from '../store';
import { Subject } from 'rxjs';
import { AuthService } from '../service/auth.service';


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
    private authService: AuthService,
    private store: Store) { }

  ngOnInit(): void {
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      console.log('currentEvent', currentEvent);
      this.yEvent = currentEvent;
    });

    this.authService.loggedIn$.pipe(takeUntil(this.destroy$)).subscribe(l => {
      this.userIsLoggedIn = l;
    });

    this.authService.isAdmin$.pipe(takeUntil(this.destroy$)).subscribe(a => {
      this.userIsAdmin = a;
    });
  }


}
