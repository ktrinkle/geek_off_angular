import {
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../../environments/environment';
import * as fromRound2 from './round2/round2.reducer';

export interface State {
  round2: fromRound2.State
}

export const reducers: ActionReducerMap<State> = {
  round2: fromRound2.reducer
};

export const metaReducers: MetaReducer<State>[] = !environment.production ? [] : [];

export const selectRound2State = createFeatureSelector<fromRound2.State>(fromRound2.round2FeatureKey)
export const selectRound2AllSurvey = createSelector(selectRound2State, (state: fromRound2.State) => state.surveyList);