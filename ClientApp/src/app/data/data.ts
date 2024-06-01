import { Guid } from "typescript-guid";

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
  teamNum: number,
  player1Answers: round2Answers[],
  player2Answers?: round2Answers[],
  finalScore: number
}

export interface round1Scores {
  teamNum: number,
  teamName: string,
  q: round1ScoreDetail[],
  bonus?: number,
  teamScore?: number,
  rnk?: number
}

export interface round1ScoreDetail {
  questionId: number,
  questionScore?: number
}

export interface round23Scores {
  teamNum: number,
  teamName: string,
  teamScore?: number,
  rnk?: number,
  color?: string
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
  teamNum: number,
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
  answerType: number,
}

export interface round1QDisplay {
  questionNum: number,
  questionText: string,
  answers: round1AnswerDto[],
  correctAnswer: string,
  answerType: number,
  mediaFile: string,
  mediaType: string
}

export interface currentQuestionDto {
  questionNum: number,
  status: number
}

export interface round1QuestionControlDto {
  questionNum: number,
  questionText: string,
  questionAnswerType: number,
  answerText: string
}

export interface round1EnteredAnswers {
  yEvent: string,
  teamNum: number,
  questionNum: number,
  textAnswer: string,
  answerStatus: boolean
}

export interface round3QuestionDto {
  questionNum: number,
  sortOrder: number,
  score: number
}

export interface round3AnswerDto {
  yEvent: string,
  questionNum: number,
  teamNum: number,
  score: number
}

export interface eventMaster {
  yEvent: string,
  eventName: string,
  selEvent: boolean
}

export interface apiResponse {
  successInd: boolean,
  response: string
}

export interface teamEntry {
  teamNum: number,
  teamGuid: Guid,
  successInd: boolean,
  teamName: string
}

export interface jwtReturn {
  teamNum: number,
  adminName: string,
  userName: string,
  sessionGuid: Guid,
  role: string
}

export interface adminLogin {
  userName: string,
  password: string,
}

export interface bearerDto {
  teamNum: number,
  teamName?: string,
  userName?: string,
  humanName: string,
  bearerToken: string,
}

export interface simpleUser {
  teamNum?: number,
  teamName?: string
}

export interface teamLogin {
  yEvent: string,
  teamGuid: Guid
}

export interface newTeamEntry {
  successInd: boolean,
  teamNum: number,
  teamGuid : Guid,
  teamName : string
}
