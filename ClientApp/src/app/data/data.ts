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

export interface round1Scores {
  teamNum: number,
  teamName: string,
  q: round1ScoreDetail[],
  teamScore?: number,
  rnk?: number
}

export interface round1ScoreDetail {
  questionId: number,
  questionScore?: number
}

export interface round23Scores {
  teamNo: number,
  teamName: string,
  teamScore?: number,
  rnk?: number
}

export interface round2SurveyList {
  questionNum: number,
  questionText: string,
  surveyAnswers: round2Answers[]
}

export interface round2SubmitAnswer {
  yEvent: string,
  questionNum: number,
  teamNum: number,
  playerNum: number,
  answerNum?: number,
  answer?: string,
  score?: number
}

export interface introDto {
  teamNo: number,
  teamName: string,
  member1: string,
  member2?: string,
  workgroup1: string,
  workgroup2?: string
}

export interface round1AnswerDto {
  answerId: number,
  answer: string
}

export interface round1QADto {
  questionNum: number,
  questionText: string,
  answers: round1AnswerDto[],
  expireTime: Date,
  questionAnswerType: number,
}

export interface currentQuestionDto {
  questionNum: number,
  status: number
}

export interface round1QuestionControlDto
{
  questionNum: number,
  questionText: string,
  questionAnswerType: number,
  answerText: string
}

export interface round1EnteredAnswers
{
  yEvent: string,
  teamNum: number,
  questionNum: number,
  textAnswer: string
}
