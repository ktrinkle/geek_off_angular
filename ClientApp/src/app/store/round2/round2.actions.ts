import { createAction, props } from '@ngrx/store';

export const round2Round2s = createAction(
  '[Round2] Round2 Round2s'
);

export const round2Round2sSuccess = createAction(
  '[Round2] Round2 Round2s Success',
  props<{ data: any }>()
);

export const round2Round2sFailure = createAction(
  '[Round2] Round2 Round2s Failure',
  props<{ error: any }>()
);
