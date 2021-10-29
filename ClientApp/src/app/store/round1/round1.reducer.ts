import { Action, createReducer, on } from '@ngrx/store';
import { introDto, round1QADto, round1QDisplay } from 'src/app/data/data';
import { currentEventSuccess, round1AllQuestionsSuccess, round1AllTeamsSuccess, round1BigDisplaySuccess } from './round1.actions';

export const round1FeatureKey = 'round1';

export interface State {
  currentEvent: string,
  allTeams: Array<introDto>,
  allQuestions: Array<round1QADto>,
  bigDisplay: Array<round1QDisplay>
}

export const initialState: State = {
  currentEvent: '',
  allTeams: new Array<introDto>(),
  allQuestions: new Array<round1QADto>(),
  bigDisplay: new Array<round1QDisplay>()
};


export const round1Reducer = createReducer(
  initialState,

  on(currentEventSuccess, (state, { currentEvent }) => ({
    ...state,
    currentEvent: currentEvent
  })),

  on(round1AllTeamsSuccess, (state, { allTeams }) => ({
    ...state,
    allTeams: allTeams
  })),

  on(round1AllQuestionsSuccess, (state, { allQuestions }) => ({
    ...state,
    allQuestions: allQuestions
  })),

  on(round1BigDisplaySuccess, (state, { allQuestions }) => ({
    ...state,
    bigDisplay: allQuestions
  }))
);


export function reducer(state: State | undefined, action: Action): any {
  return round1Reducer(state, action);
}
