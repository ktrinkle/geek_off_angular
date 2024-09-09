import { Action, createReducer, on } from '@ngrx/store';
import { round23Scores } from 'src/app/data/data';
import { round3ScoreSuccess, round3BigDisplaySuccess } from './round3.actions';

export const round3FeatureKey = 'round3';

export interface State {
  teamScores: Array<round23Scores>,

}

export const initialState: State = {
  teamScores: new Array<round23Scores>()
};

export const round3Reducer = createReducer(
  initialState,

  on(round3ScoreSuccess, (state, { teamScores }) => ({
    ...state,
    teamScores: teamScores
  })),

  on(round3BigDisplaySuccess, (state, { allQuestions }) => ({
    ...state,
    bigDisplay: allQuestions
  }))
);


export function reducer(state: State | undefined, action: Action): any {
  return round3Reducer(state, action);
}
