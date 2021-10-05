import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { round1QuestionControlDto, round1Scores, round1EnteredAnswers } from 'src/app/data/data';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';


@Component({
  selector: 'app-round1',
  templateUrl: './round1control.component.html',
  styleUrls: ['./round1control.component.scss']
})
export class Round1ControlComponent implements OnInit {

  currentQuestion: number = 0;
  possibleAnswers: round1QuestionControlDto[] = [];
  teamAnswers: round1EnteredAnswers[] = [];
  scoreboard: round1Scores[] = [];
  yEvent = sessionStorage.getItem('event') ?? '';
  statusEnum:string[] = [''];
  status: number = 0;

  constructor(private dataService: DataService) {
    this.dataService.getAllRound1Questions(this.yEvent).subscribe(q => {
      this.possibleAnswers = q;
    });

    // mostly in case of reload
    this.dataService.getCurrentQuestion(this.yEvent).subscribe(c => {
      this.status = c.status;
      this.currentQuestion = c.questionNum;
    });
  }

  ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events')
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    // commented out in case we need to keep these to keep SignalR happy
    // connection.on("round1ShowAnswerChoices", (data: any) => {});

    // connection.on("round1OpenAnswer", (data: any) => {});

    connection.on("round1PlayerAnswer", (data: any) => {
      this.loadTeamAnswers();
    });

    connection.on("round1ScoreUpdate", (data: any) => {
      this.updateScoreboard();
    })

    // connection.on("round1CloseAnswer", (data: any) => {});
  }

  loadTeamAnswers()
  {
    this.dataService.getAllEnteredAnswers(this.yEvent, this.currentQuestion).subscribe(a => {
      this.teamAnswers = a;
    });
  }

  updateScoreboard()
  {
    this.dataService.getRound1Scores(this.yEvent).subscribe({next: (a => {
      this.scoreboard = a;
    }), complete: () => {
      this.scoreboard.sort((a,b) => 0 - ((a.teamScore ?? 0) > (b.teamScore ?? 0) ? 1 : -1));
    }
  });
  }

  sendClientMessage(questionNum: number, status: number)
  {
    this.dataService.changeRound1QuestionStatus(this.yEvent, questionNum, status).subscribe(c => {
      this.status = c.status;
      this.currentQuestion = c.questionNum;
    });
  }

}
