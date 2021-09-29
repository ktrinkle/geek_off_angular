import { Injectable } from '@angular/core';
import { Actions, createEffect, Effect, ofType } from '@ngrx/effects';
import { catchError, map, mergeMap, switchMap } from 'rxjs/operators';
import { round1AllTeams, round1AllTeamsSuccess } from './round1.actions';
import { DataService } from 'src/app/data.service';
import { of } from 'rxjs';

@Injectable()
export class Round1Effects {

  constructor(private actions$: Actions, private dataService: DataService) { }

  getAllTeams$ = createEffect(() => this.actions$.pipe(
    ofType(round1AllTeams),
    switchMap(payload =>
      this.dataService.getRound1IntroTeamList(payload.yEvent).pipe(map(teams =>
        round1AllTeamsSuccess({ allTeams: teams }) // todo: add catchError
      )))));
}
