import { Component, OnInit } from '@angular/core';
import { round1QADto, round1EnteredAnswers, currentQuestionDto } from 'src/app/data/data';
import { DataService } from 'src/app/data.service';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-round1host',
  templateUrl: './round1host.component.html',
  styleUrls: ['./round1host.component.scss']
})
export class Round1hostComponent implements OnInit {

  teamAnswers: round1EnteredAnswers[] = [];
  yEvent = sessionStorage.getItem('event') ?? '';
  public currentQuestion: currentQuestionDto = {
    questionNum: 0,
    status: 0
  };
  public currentQuestionDto: round1QADto = {
    questionNum: 0,
    questionText: '',
    answers: [],
    answerType: -1,
    expireTime: new Date()
  };

  constructor(private dataService: DataService) { }

  ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events')
      .withAutomaticReconnect()
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round1question", (data: any) => {
      this.loadQuestion(data);
    });

    connection.on("round1PlayerAnswer", (data: any) => {
      this.loadTeamAnswers();
    });

    this.dataService.getCurrentQuestion(this.yEvent).subscribe({next: (initialQ => {
      this.currentQuestion = initialQ;
    }), complete: () => {
      console.log(this.currentQuestion);
      if (this.currentQuestion.questionNum > 0) {
        this.loadQuestion(this.currentQuestion.questionNum);
        this.loadTeamAnswers();
      };
    }});

    // connection.on("round1CloseAnswer", (data: any) => {});
  }

  loadTeamAnswers()
  {
    this.dataService.getAllEnteredAnswers(this.yEvent, this.currentQuestion.questionNum).subscribe(a => {
      this.teamAnswers = a;
    });
  }

  loadQuestion(questionId: number): void{
    this.dataService.getRound1QuestionAnswer(this.yEvent, questionId).subscribe(q => {
        this.currentQuestionDto = q;
      });
  };
}
