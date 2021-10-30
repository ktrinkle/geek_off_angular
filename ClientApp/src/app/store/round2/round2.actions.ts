import { createAction, props } from '@ngrx/store';
import { round2SurveyList } from 'src/app/data/data';

export const round2AllSurvey = createAction(
  '[Round2] GetAllSurvey',
  props<{ yEvent: string }>()
);

export const round2AllSurveySuccess = createAction(
  '[Round2] LoadAllSurvey',
  props<{ allSurvey: round2SurveyList[] }>()
);
