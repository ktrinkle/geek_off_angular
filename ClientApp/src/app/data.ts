export interface round2SurveyQuestions
{
  questionNo: number,
  surveyAnswers: round2Answers[]
}

export interface round2Answers
{
  questionNo: number,
  answer: string,
  score: number
}
