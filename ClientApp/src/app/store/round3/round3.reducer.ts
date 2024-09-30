import { Action, createReducer, on } from '@ngrx/store';
import { round1QDisplay, round23Scores, roundCategory, roundThreeCategoryPoints } from 'src/app/data/data';
import { round3ScoreSuccess, round3BigDisplaySuccess, round3CategoriesSuccess, round3CategoryPointsSuccess } from './round3.actions';

export const round3FeatureKey = 'round3';

export interface State {
  teamScores: Array<round23Scores>,
  bigDisplay: Array<round1QDisplay>,
  categories: Array<roundCategory>,
  questions: Array<roundThreeCategoryPoints>
}

export const initialState: State = {
  teamScores: new Array<round23Scores>(),
  bigDisplay: new Array<round1QDisplay>(),
  categories: new Array<roundCategory>(),
  questions: new Array<roundThreeCategoryPoints>()
};

export const round3Reducer = createReducer(
  initialState,

  on(round3ScoreSuccess, (state, { teamScores }) => ({
    ...state,
    teamScores: teamScores
  })),

  on(round3BigDisplaySuccess, (state, { allQuestions }) => ({
    ...state,
    bigDisplay: allQuestions
  })),

  on(round3CategoriesSuccess, (state, { allCategories }) => ({
    ...state,
    categories: allCategories
  })),

  on(round3CategoryPointsSuccess, (state, { allQuestions }) => ({
    ...state,
    questions: allQuestions
  }))
);


export function reducer(state: State | undefined, action: Action) {
  return round3Reducer(state, action);
}
