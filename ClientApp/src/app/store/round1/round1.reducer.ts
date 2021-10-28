import { Action, createReducer, on } from '@ngrx/store';
import { introDto, round1QADto } from 'src/app/data/data';
import { round1AllQuestionsSuccess, round1AllTeamsSuccess } from './round1.actions';

export const round1FeatureKey = 'round1';

export interface State {
  allTeams: Array<introDto>,
  allQuestions: Array<round1QADto>
}

export const initialState: State = {
  allTeams: new Array<introDto>(),
  allQuestions: new Array<round1QADto>()
};


export const round1Reducer = createReducer(
  initialState,

  // on(round2AllSurvey, (state) => ({
  //   ...state,
  // })),

  on(round1AllTeamsSuccess, (state, { allTeams }) => ({
    ...state,
    allTeams: allTeams
  })),

  on(round1AllQuestionsSuccess, (state, { allQuestions }) => ({
    ...state,
    allQuestions: allQuestions
  }))
);


export function reducer(state: State | undefined, action: Action): any {
  return round1Reducer(state, action);
}
