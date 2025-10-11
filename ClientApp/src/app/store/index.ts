import {
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer
} from '@ngrx/store';
import { environment } from '../../environments/environment';
import * as fromRound1 from './round1/round1.reducer';
import * as fromRound2 from './round2/round2.reducer';
import * as fromRound3 from './round3/round3.reducer';
import * as fromEventManage from './eventManage/eventManage.reducer';

export interface State {
  round1: fromRound1.State,
  round2: fromRound2.State,
  round3: fromRound3.State,
  eventManage: fromEventManage.State
}

export const reducers: ActionReducerMap<State> = {
  round1: fromRound1.reducer,
  round2: fromRound2.reducer,
  round3: fromRound3.reducer,
  eventManage: fromEventManage.reducer
};

export const metaReducers: MetaReducer<State>[] = !environment.production ? [] : [];

export const selectRound1State = createFeatureSelector<fromRound1.State>(fromRound1.round1FeatureKey);
export const selectRound1Teams = createSelector(selectRound1State, (state: fromRound1.State) => state.allTeams);
export const selectRound2State = createFeatureSelector<fromRound2.State>(fromRound2.round2FeatureKey);
export const selectRound2AllSurvey = createSelector(selectRound2State, (state: fromRound2.State) => state.surveyList);
export const selectRound3State = createFeatureSelector<fromRound3.State>(fromRound3.round3FeatureKey);
export const selectRound3Scores = createSelector(selectRound3State, (state: fromRound3.State) => state.teamScores);
export const selectRound1AllQuestionsAndAnswers = createSelector(selectRound1State, (state: fromRound1.State) => state.allQuestions);
export const selectRound1BigDisplay = createSelector(selectRound1State, (state: fromRound1.State) => state.bigDisplay);

// event manage store
export const selectEventManageState = createFeatureSelector<fromEventManage.State>(fromEventManage.eventManageFeatureKey);
export const selectCurrentEvent = createSelector(selectEventManageState, (state: fromEventManage.State) => state.currentEvent);
export const selectAllEvents = createSelector(selectEventManageState, (state: fromEventManage.State) => state.allEvents);

