import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { map, switchMap } from 'rxjs/operators';
import { round1AllQuestions, round1AllQuestionsSuccess, round1AllTeams, round1AllTeamsSuccess, round1BigDisplay, round1BigDisplaySuccess } from './round1.actions';
import { DataService } from 'src/app/data.service';

@Injectable()
export class Round1Effects {

  constructor(private actions$: Actions, private dataService: DataService) { }

  getAllTeams$ = createEffect(() => this.actions$.pipe(
    ofType(round1AllTeams),
    switchMap(payload =>
      this.dataService.getRound1IntroTeamList(payload.yEvent).pipe(map(teams =>
        round1AllTeamsSuccess({ allTeams: teams }) // todo: add catchError
      )))));

  getRound1AllQuestions$ = createEffect(() => this.actions$.pipe(
    ofType(round1AllQuestions),
    switchMap(payload =>
      this.dataService.getAllRound1QuestionsAndAnswers(payload.yEvent).pipe(map(allQuestions =>
        round1AllQuestionsSuccess({ allQuestions: allQuestions }) // todo: add catchError
      )))));

  // List<round1QDisplay>
  getRound1BigDisplay$ = createEffect(() => this.actions$.pipe(
    ofType(round1BigDisplay),
    switchMap(payload =>
      this.dataService.getRound1BigDisplay(payload.yEvent).pipe(map(allQuestions =>
        round1BigDisplaySuccess({ allQuestions: allQuestions }) // todo: add catchError
      )))));
}
