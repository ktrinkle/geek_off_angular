import { Action, createReducer, on } from '@ngrx/store';
import { introDto } from 'src/app/data/data';
import { round1AllTeamsSuccess } from './round1.actions';

export const round1FeatureKey = 'round1';

export interface State {
  allTeams: Array<introDto>,

}

export const initialState: State = {
  allTeams: new Array<introDto>()
};


export const round1Reducer = createReducer(
  initialState,

  // on(round2AllSurvey, (state) => ({
  //   ...state,
  // })),

  on(round1AllTeamsSuccess, (state, { allTeams }) => ({
    ...state,
    allTeams: allTeams
  }))
);


export function reducer(state: State | undefined, action: Action): any {
  return round1Reducer(state, action);
}
