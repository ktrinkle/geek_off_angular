import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { map, switchMap } from 'rxjs/operators';
import { round3Score, round3ScoreSuccess, round3BigDisplay, round3BigDisplaySuccess, round3Categories, round3CategoriesSuccess, round3CategoryPoints, round3CategoryPointsSuccess } from './round3.actions';
import { DataService } from 'src/app/data.service';

@Injectable()
export class Round3Effects {

  constructor(private actions$: Actions, private dataService: DataService) { }

  getRound3SurveyQuestions$ = createEffect(() => this.actions$.pipe(
    ofType(round3Score),
    switchMap(payload =>
      this.dataService.getRound3Scores(payload.yEvent).pipe(map(teamScores =>
        round3ScoreSuccess({ teamScores: teamScores }) // todo: add catchError
      )))));

    // List<round1QDisplay>
    getRound3BigDisplay$ = createEffect(() => this.actions$.pipe(
      ofType(round3BigDisplay),
      switchMap(payload =>
        this.dataService.getRound3BigDisplay(payload.yEvent).pipe(map(allQuestions =>
          round3BigDisplaySuccess({ allQuestions: allQuestions }) // todo: add catchError
        )))));

    getRound3Categories$ = createEffect(() => this.actions$.pipe(
      ofType(round3Categories),
      switchMap(payload =>
        this.dataService.getRound3Categories(payload.yEvent).pipe(map(categories =>
          round3CategoriesSuccess({ allCategories: categories })
        )))));

    getRound3CategoryPoints$ = createEffect(() => this.actions$.pipe(
      ofType(round3CategoryPoints),
      switchMap(payload =>
        this.dataService.getRound3CategoryPoints(payload.yEvent).pipe(map(questions =>
          round3CategoryPointsSuccess({ allQuestions: questions })
        )))));
}
