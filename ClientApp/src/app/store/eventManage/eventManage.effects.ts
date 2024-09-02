import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { currentEvent, currentEventSuccess, allEvent, allEventSuccess } from './eventManage.actions';
import { DataService } from 'src/app/data.service';
import { EMPTY } from 'rxjs';

@Injectable()
export class EventManageEffects {

  constructor(private actions$: Actions, private dataService: DataService) { }

  getCurrentEvent$ = createEffect(() => this.actions$.pipe(
    ofType(currentEvent),
    switchMap(() =>
      this.dataService.getCurrentEvent().pipe(map(currentEvent =>
        currentEventSuccess({ currentEvent: currentEvent }) // todo: add catchError
      )))));

    // exhaustMap(() =>
    //   this.dataService.getCurrentEvent().pipe(map(currentEvent => ({
    //       type: '[EventManage] LoadCurrentEvent', payload: currentEvent })),
    //       catchError(() => EMPTY)  // todo: add catchError
    //   ))));

  getAllEvents$ = createEffect(() => this.actions$.pipe(
    ofType(allEvent),
    switchMap(payload =>
      this.dataService.getAllEvents().pipe(map(eventList =>
        allEventSuccess({ eventList: eventList }) // todo: add catchError
      )))));
}
