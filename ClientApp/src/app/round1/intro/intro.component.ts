import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { Store } from '@ngrx/store';
import { round1AllTeams, round1AllTeamsSuccess } from '../../store/round1/round1.actions';
import { introDto } from '../../data/data';
import { selectRound1Teams } from 'src/app/store';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

@Component({
  selector: 'app-intro',
  templateUrl: './intro.component.html',
  styleUrls: ['./intro.component.scss'],
  animations: []
})
export class Round1IntroComponent implements OnInit, OnDestroy {

  currentScreen: string = "";
  public yevent: string = 'e21';
  public teamMasterList: introDto[] = [];
  constructor(private store: Store, private route: ActivatedRoute) {
    this.store.dispatch(round1AllTeams({ yEvent: 'e21' }));
   }

   destroy$: Subject<boolean> = new Subject<boolean>();

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.currentScreen = params['page'];
      console.log('Screen: ' + this.currentScreen);
    });

    this.store.select(selectRound1Teams).pipe(takeUntil(this.destroy$)).subscribe(x =>
      this.teamMasterList = x
    );
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

}
