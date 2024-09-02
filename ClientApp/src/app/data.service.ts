import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from './../environments/environment';
import { newTeamEntry, round2SubmitAnswer, round3AnswerDto, round3QuestionDto, eventMaster, apiResponse, adminLogin, bearerDto, teamLogin } from './data/data';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private REST_API_SERVER = environment.api_url;

  constructor(private httpClient: HttpClient) { }

  // login stuff
  // need to embed in AdminLogin:{ userLogin fields }
  public sendAdminLogin(userLogin: adminLogin): Observable<bearerDto> {
    var uri = this.REST_API_SERVER + '/api/login/admin';
    return this.httpClient.post<bearerDto>(uri, userLogin);
  }

  public sendTeamLogin(userLogin: teamLogin): Observable<bearerDto> {
    var uri = this.REST_API_SERVER + '/api/login/team/' + encodeURIComponent(userLogin.yEvent)
              + '/' + encodeURIComponent(userLogin.teamGuid.toString()) + '';
    return this.httpClient.put<bearerDto>(uri, {});
  }

  // team link stuff

  public createTeamLink(yevent: string, teamName: string): Observable<newTeamEntry> {
    var uri = this.REST_API_SERVER + '/api/eventmanage/createTeam/' + encodeURIComponent(yevent)
              + '/' + encodeURIComponent(teamName) + '';
    return this.httpClient.put<newTeamEntry>(uri, {});
  }

  public moveTeamNumber(yevent: string, oldTeamNum: number, newTeamNum: number): Observable<apiResponse> {
    var uri = this.REST_API_SERVER + '/api/eventmanage/createTeam/' + encodeURIComponent(yevent)
              + '/' + encodeURIComponent(oldTeamNum) + '/' + encodeURIComponent(newTeamNum) + '';
    return this.httpClient.put<apiResponse>(uri, {});
  }

  public listTeamAndLink(yevent: string): Observable<newTeamEntry[]> {
    var uri = this.REST_API_SERVER + '/api/eventmanage/listTeamAndLink/' + encodeURIComponent(yevent)
              + '';
    return this.httpClient.put<newTeamEntry[]>(uri, {});
  }

  // event stuff
  public getCurrentEvent(): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/eventStatus/currentEvent';
    return this.httpClient.get(uri, { responseType: "text" });
  }

  public getAllEvents(): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/eventmanage/eventList';
    return this.httpClient.get(uri);
  }

  public setCurrentEvent(yevent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/eventmanage/setEvent/' + encodeURIComponent(yevent) + '';
    return this.httpClient.put(uri, {});
  }

  public addNewEvent(newEvent: eventMaster): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/eventmanage/addEvent';
    return this.httpClient.post(uri, newEvent);
  }

  public getCurrentQuestion(yevent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/eventStatus/currentQuestion/' + encodeURIComponent(yevent) + '';
    return this.httpClient.get(uri);
  }

  public getAllRound2FeudSurveyQuestions(yevent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2_feud/allSurvey/' + encodeURIComponent(yevent) + '';
    return this.httpClient.get(uri);
  }

  public sendRound2FeudAnswerText(submitAnswer: round2SubmitAnswer): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2_feud/teamanswer/text';
    return this.httpClient.post(uri, submitAnswer, { responseType: 'text' });
  }

  public getRound1IntroTeamList(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/teamList/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.get(uri);
  }

  public getRound1BigDisplay(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/bigDisplay/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.get(uri);
  }

  public getRound1QuestionAnswer(yEvent: string, questionNum: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/getAnswers/' + encodeURIComponent(yEvent) + '/' + encodeURIComponent(questionNum) + '';
    return this.httpClient.get(uri);
  }

  public getRound2FeudScores(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2_feud/scoreboard/' + encodeURIComponent(yEvent);
    return this.httpClient.get(uri);
  }

  public getRound2FeudDisplayBoard(yEvent: string, teamNumber: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2_feud/bigBoard/' + encodeURIComponent(yEvent) + '/' + encodeURIComponent(teamNumber);
    return this.httpClient.get(uri);
  }

  public getAllRound1Questions(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/getAllQuestions/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.get(uri);
  }

  public getAllRound1QuestionsAndAnswers(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/getAnswerList/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.get(uri);
  }

  public getAllEnteredAnswers(yEvent: string, questionNum: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/showTeamAnswer/' + encodeURIComponent(yEvent) + '/' + encodeURIComponent(questionNum) + '';
    return this.httpClient.get(uri);
  }

  public saveRound1Answer(yEvent: string, questionNum: number, textAnswer: string): Observable<string> {
    var uri = this.REST_API_SERVER + '/api/round1/submitAnswer';
    let params = { yevent: yEvent, questionNum: questionNum, textAnswer: textAnswer };
    return this.httpClient.put(uri, params, { responseType: 'text' });
  }

  public changeRound1QuestionStatus(yEvent: string, questionNum: number, status: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/updateStatus/' + encodeURIComponent(yEvent) + '/' + encodeURIComponent(questionNum) + '/' + encodeURIComponent(status) + '';
    return this.httpClient.put(uri, { responseType: 'json' });
  }

  public showRound1QuestionMedia(questionNum: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/showMedia/' + encodeURIComponent(questionNum) + '';
    return this.httpClient.put(uri, { responseType: 'json' });
  }

  public getRound1Scores(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/scoreboard/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.get(uri);
  }

  public updateScoreboardDisplay() {
    var uri = this.REST_API_SERVER + '/api/round1/updateScoreboard';
    this.httpClient.put(uri, {}).toPromise();
  }

  public async updateScoreboardRound2Feud(): Promise<void> {
    var uri = this.REST_API_SERVER + '/api/round2_feud/updateScoreboard';
    await this.httpClient.get(uri).toPromise();
  }

  public finalizeRound1(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/finalize/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.put(uri, {}, { responseType: 'text' });
  }

  public finalizeRound2Feud(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2_feud/finalize/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.put(uri, {}, { responseType: 'text' });
  }

  public round1AutoScore(yEvent: string, questionNum: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round1/scoreAnswer/' + encodeURIComponent(yEvent) + '/' + encodeURIComponent(questionNum) + '';
    console.log(uri);
    return this.httpClient.put(uri, {}, { responseType: 'text' });
  }

  public round1ManualScore(yEvent: string, questionNum: number, teamNum: number): Promise<any> {
    var uri = this.REST_API_SERVER + '/api/round1/scoreManualAnswer/' + encodeURIComponent(yEvent) + '/' + encodeURIComponent(questionNum) + '/' + encodeURIComponent(teamNum) + '';
    return this.httpClient.put(uri, {}, { responseType: 'text' }).toPromise();
  }

  public changeIntroPage(page: string): Promise<any> {
    var uri = this.REST_API_SERVER + '/api/round1/changeIntroPage/' + encodeURIComponent(page) + '';
    console.log(uri);
    return this.httpClient.put(uri, { responseType: 'json' }).toPromise();
  }

  public changeAnimation(): Promise<any> {
    var uri = this.REST_API_SERVER + '/api/round1/animateText';
    return this.httpClient.put(uri, { responseType: 'json' }).toPromise();
  }

  public changeSeatbelt(): Promise<any> {
    var uri = this.REST_API_SERVER + '/api/round1/animateSeatbelt';
    return this.httpClient.put(uri, { responseType: 'json' }).toPromise();
  }

  public revealRound2FeudValue(entryNum: number) {
    var uri = this.REST_API_SERVER + '/api/round2_feud/bigboard/reveal/' + encodeURIComponent(entryNum) + '';
    this.httpClient.get(uri).toPromise();
  }

  public getRound2FeudFirstPlayer(yEvent: string, teamNum: number): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2_feud/firstPlayersAnswers/' + encodeURIComponent(yEvent) + '/' + encodeURIComponent(teamNum) + '';
    return this.httpClient.get(uri);
  }

  public revealRound2FeudPlayer1() {
    var uri = this.REST_API_SERVER + '/api/round2_feud/bigboard/playerone';
    this.httpClient.get(uri).toPromise();
  }

  public changeRound2FeudTeam(teamNum: any) {
    console.log('Dataservice TeamNum:' + teamNum);
    var uri = this.REST_API_SERVER + '/api/round2_feud/bigboard/changeTeam/' + encodeURIComponent(teamNum) + '';
    this.httpClient.get(uri).toPromise();
  }

  public startCountdown(seconds: number) {
    var uri = this.REST_API_SERVER + '/api/round2_feud/countdown/start/' + + encodeURIComponent(seconds);
    this.httpClient.get(uri).toPromise();
  }

  public stopCountdown() {
    var uri = this.REST_API_SERVER + '/api/round2_feud/countdown/stop';
    this.httpClient.get(uri).toPromise();
  }

  public setCountdown(seconds: number) {
    var uri = this.REST_API_SERVER + '/api/round2_feud/countdown/set/' + encodeURIComponent(seconds);
    this.httpClient.get(uri).toPromise();
  }

  public changeRound2FeudPage(page: string): Promise<any> {
    var uri = this.REST_API_SERVER + '/api/round2_feud/changePage/' + encodeURIComponent(page) + '';
    return this.httpClient.put(uri, { responseType: 'json' }).toPromise();
  }

  public changeContestantStatus(questionNum: number, status: number) {
    var uri = this.REST_API_SERVER + '/api/round1/updateState/' + encodeURIComponent(questionNum) + '/' + encodeURIComponent(status) + '';
    this.httpClient.put(uri, {}, { responseType: 'json' }).toPromise();
  }

  public updateDollarAmount(yevent: string, teamNum: number, dollarAmount: number): Observable<any> {
    var params = new HttpParams().set('dollarAmount', dollarAmount);
    var uri = this.REST_API_SERVER + '/api/eventstatus/dollarAmount/' + encodeURIComponent(yevent) + '/' + encodeURIComponent(teamNum) + '';
    return this.httpClient.put(uri, {}, { params: params, responseType: 'text' });
  }

  public updateRound3Scores(scores: round3AnswerDto[]): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round3/teamAnswer';
    return this.httpClient.post(uri, scores, { responseType: 'text' });
  }

  public getAllRound3Questions(yevent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round3/allQuestions/' + encodeURIComponent(yevent) + '';
    return this.httpClient.get(uri);
  }

  public getAllRound3Teams(yevent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round3/allTeams/' + encodeURIComponent(yevent) + '';
    return this.httpClient.get(uri);
  }

  public getRound3Scores(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round3/scoreboard/' + encodeURIComponent(yEvent);
    return this.httpClient.get(uri);
  }

  public async updateScoreboardRound3(): Promise<void> {
    var uri = this.REST_API_SERVER + '/api/round3/updateScoreboard';
    await this.httpClient.get(uri).toPromise();
  }

  public finalizeRound3(yEvent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round3/finalize/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.put(uri, {}, { responseType: 'text' });
  }

  public cleanEventData(yEvent: string): Promise<any> {
    var uri = this.REST_API_SERVER + '/api/eventmanage/cleanEvent/' + encodeURIComponent(yEvent) + '';
    return this.httpClient.put(uri, {}, { responseType: 'text' }).toPromise();
  }

}
