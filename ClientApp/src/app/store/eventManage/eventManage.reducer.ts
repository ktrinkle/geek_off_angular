import { Action, createReducer, on } from '@ngrx/store';
import { eventMaster } from 'src/app/data/data';
import { allEventSuccess, currentEventSuccess } from './eventManage.actions';

export const eventManageFeatureKey = 'eventManage';

export interface State {
  currentEvent: string,
  allEvents: Array<eventMaster>
}

export const initialState: State = {
  currentEvent: '',
  allEvents: new Array<eventMaster>()
};


export const eventManageReducer = createReducer(
  initialState,

  on(currentEventSuccess, (state, { currentEvent }) => ({
    ...state,
    currentEvent: currentEvent
  })),

  on(allEventSuccess, (state, { eventList }) => ({
    ...state,
    eventList: eventList
  })),

);

export function reducer(state: State | undefined, action: Action) {
  return eventManageReducer(state, action);
}
