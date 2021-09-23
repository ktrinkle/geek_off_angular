import { createAction, props } from '@ngrx/store';
import { round2SurveyList } from 'src/app/data/data';

export const round2AllSurvey = createAction(
  '[Round2] GetAllSurvey',
  props<{ payload: { yEvent: string } }>()
);

export const round2AllSurveySuccess = createAction(
  '[Round2] LoadAllSurvey',
  props<{ allSurvey: round2SurveyList[] }>()
);

// export const round2Round2sFailure = createAction(
//   '[Round2] Round2 Round2s Failure',
//   props<{ error: any }>()
// );
