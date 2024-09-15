import { createAction, props } from '@ngrx/store';
import { round1QDisplay, round23Scores } from 'src/app/data/data';

export const round3Score = createAction(
  '[Round3] GetAllScores',
  props<{ yEvent: string }>()
);

export const round3ScoreSuccess = createAction(
  '[Round3] LoadAllScores',
  props<{ teamScores: round23Scores[] }>()
);

export const round3BigDisplay = createAction(
  '[Round1] GetRound3BigDisplay',
  props<{ yEvent: string }>()
);

export const round3BigDisplaySuccess = createAction(
  '[Round1] LoadRound3BigDisplay',
  props<{ allQuestions: round1QDisplay[] }>()
);
