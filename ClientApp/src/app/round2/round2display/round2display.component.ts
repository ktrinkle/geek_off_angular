import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { round2SurveyList } from 'src/app/data/data';
import { selectRound2AllSurvey } from 'src/app/store';
import { round2AllSurvey } from 'src/app/store/round2/round2.actions';

@Component({
  selector: 'app-round2display',
  templateUrl: './round2display.component.html',
  styleUrls: ['./round2display.component.scss']
})
export class Round2displayComponent implements OnInit, OnDestroy {
  destroy$: Subject<boolean> = new Subject<boolean>();
  something: any;

  constructor(private store: Store) {
    this.store.dispatch(round2AllSurvey({ yEvent: 'e21' }))
  }


  ngOnInit(): void {
    this.store.select(selectRound2AllSurvey).pipe(takeUntil(this.destroy$)).subscribe(x =>
      this.something = x
    );
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

}
