import { createAction, props } from '@ngrx/store';
import { eventMaster } from '../../data/data';


export const currentEvent = createAction(
  '[EventManage] getCurrentEvent');

export const currentEventSuccess = createAction(
  '[EventManage] LoadCurrentEvent',
  props<{ currentEvent: string }>()
);

export const allEvent = createAction(
  '[EventManage] getAllEvent');

export const allEventSuccess = createAction(
  '[EventManage] loadAllEvent',
  props<{ eventList: eventMaster[] }>()
);
