import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from './../environments/environment';
import { round2SurveyList, round2SubmitAnswer } from './data/data';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private REST_API_SERVER = environment.api_url;
  public currentEvent: string = '';

  constructor(private httpClient: HttpClient) { }

  public getAzureAuth() {
    var url = '/.auth/me';
    return this.httpClient.get(url);
  }

  public getADProfile(): Observable<any> {
    var uri = 'https://graph.microsoft.com/v1.0/me'
    return this.httpClient.get(uri);
  }

  public getCurrentEvent(): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/eventStatus/currentEvent';
    return this.httpClient.get(uri, {responseType: "text"});
  }

  public getCurrentQuestion(yevent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/eventStatus/currentQuestion/' + encodeURIComponent(yevent) + '';
    return this.httpClient.get(uri);
  }

  public getAllRound2SurveyQuestions(yevent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2/allSurvey/' + encodeURIComponent(yevent) + '';
    return this.httpClient.get(uri);
  }

  public sendRound2AnswerText(submitAnswer: round2SubmitAnswer): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2/teamanswer/text';
    return this.httpClient.post(uri, submitAnswer, {responseType: 'text'});
  }

  public getRound1IntroTeamList(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/teamList/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.get(uri);
  }

  public getRound1QuestionText(yEvent: string, questionId: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/getQuestion/' + encodeURIComponent(yEvent) + '/'  + encodeURIComponent(questionId) + '';
    return this.httpClient.get(uri);
  }

  public getRound1QuestionAnswer(yEvent: string, questionId: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/getAnswers/' + encodeURIComponent(yEvent) + '/'  + encodeURIComponent(questionId) + '';
    return this.httpClient.get(uri);
  }

  public getRound2Scores(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2/scoreboard/' + encodeURIComponent(yEvent);
    return this.httpClient.get(uri);
  }
}
