import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from './../environments/environment';
import { round2SurveyList } from './data/data';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private REST_API_SERVER = environment.api_url;

  constructor(private httpClient: HttpClient) { }

  public getAzureAuth() {
    var url = '/.auth/me';
    return this.httpClient.get(url);
  }

  public getAllRound2SurveyQuestions(yevent: string): Observable<any> {
    var uri = this.REST_API_SERVER + '/api/round2/allSurvey/' + encodeURIComponent(yevent) + '';
    return this.httpClient.get(uri);
  }

}
