import { Action, createReducer, on } from '@ngrx/store';
import { round2SurveyQuestions } from 'src/app/data/data';


export const round2FeatureKey = 'round2';

export interface State {
  surverylist: Array<Round2SurveyList>,

}

export const initialState: State = {
  SurveryList = new Array < round2SurveyQuestions
};


export const reducer = createReducer(
  initialState,

);

