import {
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../../environments/environment';
import * as fromRound1 from './round1/round1.reducer';
import * as fromRound2 from './round2/round2.reducer';

export interface State {
  round1: fromRound1.State,
  round2: fromRound2.State
}

export const reducers: ActionReducerMap<State> = {
  round1: fromRound1.reducer,
  round2: fromRound2.reducer
};

export const metaReducers: MetaReducer<State>[] = !environment.production ? [] : [];

export const selectRound1State = createFeatureSelector<fromRound1.State>(fromRound1.round1FeatureKey);
export const selectCurrentEvent = createSelector(selectRound1State, (state: fromRound1.State) => state.currentEvent);
export const selectRound1Teams = createSelector(selectRound1State, (state: fromRound1.State) => state.allTeams);
export const selectRound2State = createFeatureSelector<fromRound2.State>(fromRound2.round2FeatureKey);
export const selectRound2AllSurvey = createSelector(selectRound2State, (state: fromRound2.State) => state.surveyList);
export const selectRound1AllQuestionsAndAnswers = createSelector(selectRound1State, (state: fromRound1.State) => state.allQuestions);
export const selectRound1BigDisplay = createSelector(selectRound1State, (state: fromRound1.State) => state.bigDisplay);

