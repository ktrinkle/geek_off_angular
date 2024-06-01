import { round2SurveyList } from './../../data/data';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
import { Store } from '@ngrx/store';
import { Subject } from 'rxjs';
import { selectCurrentEvent } from 'src/app/store';
import { takeUntil } from 'rxjs/operators';


@Component({
  selector: 'app-round2host',
  templateUrl: './round2host.component.html',
  styleUrls: ['./round2host.component.scss']
})

export class Round2hostComponent implements OnInit, OnDestroy {

  public yEvent: string = '';
  public surveyMasterList: round2SurveyList[] = [];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private _dataService: DataService, private store: Store) { }


  ngOnInit(): void {
    // Grabs all the questions.
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this._dataService.getAllRound2FeudSurveyQuestions(this.yEvent).subscribe((data: round2SurveyList[]) => {
          this.surveyMasterList = data;
        });
      }
    });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
