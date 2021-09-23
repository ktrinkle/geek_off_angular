import { Injectable } from '@angular/core';
import { Actions, createEffect, Effect, ofType } from '@ngrx/effects';
import { map, mergeMap } from 'rxjs/operators';
import { round2AllSurvey, round2AllSurveySuccess } from './round2.actions';
import { DataService } from 'src/app/data.service';

@Injectable()
export class Round2Effects {

  constructor(private actions$: Actions, private dataService: DataService) { }

  getRound2SurveyQuestions$ = this.actions$.pipe(
    ofType(round2AllSurvey),
    mergeMap(() => {
      return this.dataService.getAllRound2SurveyQuestion('2020').pipe(map(surveyQuestions => {
        round2AllSurveySuccess(surveyQuestions);
      }));
    }));
}
