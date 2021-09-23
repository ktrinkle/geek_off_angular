import { createReducer, on } from '@ngrx/store';
import { round2SurveyList } from 'src/app/data/data';
import { round2AllSurveySuccess } from './round2.actions';

export const round2FeatureKey = 'round2';

export interface State {
  surveyList: Array<round2SurveyList>,

}

export const initialState: State = {
  surveyList: new Array<round2SurveyList>()
};


export const reducer = createReducer(
  initialState,

  on(round2AllSurveySuccess, (state, { allSurvey }) => ({
    ...state,
    surveyList: allSurvey
  }))
);

