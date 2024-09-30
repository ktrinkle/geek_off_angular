import { createAction, props } from '@ngrx/store';
import { round1QDisplay, round23Scores, roundCategory, roundThreeCategoryPoints } from 'src/app/data/data';

export const round3Score = createAction(
  '[Round3] GetAllScores',
  props<{ yEvent: string }>()
);

export const round3ScoreSuccess = createAction(
  '[Round3] LoadAllScores',
  props<{ teamScores: round23Scores[] }>()
);

export const round3BigDisplay = createAction(
  '[Round3] GetRound3BigDisplay',
  props<{ yEvent: string }>()
);

export const round3BigDisplaySuccess = createAction(
  '[Round3] LoadRound3BigDisplay',
  props<{ allQuestions: round1QDisplay[] }>()
);

export const round3Categories = createAction(
  '[Round3] GetRound3Categories',
  props<{ yEvent: string }>()
)

export const round3CategoriesSuccess = createAction(
  '[Round3] LoadRound3Categories',
  props<{ allCategories: roundCategory[] }>()
)

export const round3CategoryPoints = createAction(
  '[Round3] GetRound3CategoriesPoints',
  props<{ yEvent: string }>()
)

export const round3CategoryPointsSuccess = createAction(
  '[Round3] LoadRound3CategoriesPoints',
  props<{ allQuestions: roundThreeCategoryPoints[] }>()
)
