export interface round2SurveyQuestions {
  questionNum: number,
  surveyAnswers?: round2Answers[]
}

export interface round2Answers {
  questionNum: number,
  answer: string,
  score: number
}

export interface round2Display {
  teamNo: number,
  player1Answers: round2Answers[],
  player2Answers?: round2Answers[],
  finalScore: number
}

export interface round23Scores {
  teamNo: number,
  teamName: string,
  teamScore?: number,
  rnk?: number
}

export interface round2SurveyList
{
  questionNum: number,
  questionText: string,
  surveyAnswers: round2Answers[]
}

export interface round2SubmitAnswer
{
  yEvent: string,
  questionNum: number,
  teamNum: number,
  playerNum: number,
  answerNum?: number,
  answer?: string,
  score?: number
}




