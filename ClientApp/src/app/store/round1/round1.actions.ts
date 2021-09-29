import { createAction, props } from '@ngrx/store';
import { introDto } from '../../data/data';

export const round1AllTeams = createAction(
  '[Round1] GetAllTeams',
  props<{ yEvent: string }>()
);

export const round1AllTeamsSuccess = createAction(
  '[Round1] LoadAllTeams',
  props<{ allTeams: introDto[] }>()
);
