import { createAction, props } from '@ngrx/store';
import { introDto, round1QADto } from '../../data/data';

export const round1AllTeams = createAction(
  '[Round1] GetAllTeams',
  props<{ yEvent: string }>()
);

export const round1AllTeamsSuccess = createAction(
  '[Round1] LoadAllTeams',
  props<{ allTeams: introDto[] }>()
);

export const round1AllQuestions = createAction(
  '[Round1] GetRound1AllQuestions',
  props<{ yEvent: string }>()
);

export const round1AllQuestionsSuccess = createAction(
  '[Round1] LoadRound1AllQuestions',
  props<{ allQuestions: round1QADto[] }>()
);

