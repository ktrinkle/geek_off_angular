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
  selectedQuestion: number = 0;
  possibleAnswers: round1QuestionControlDto[] = [];
  teamAnswers: round1EnteredAnswers[] = [];
  scoreboard: round1Scores[] = [];
  yEvent = sessionStorage.getItem('event') ?? '';
  statusEnum:string[] = [''];
  status: number = 0;
  scoreResponse:string = '';

  public answerForm: FormGroup = new FormGroup({
    selectedQuestion: new FormControl(this.currentQuestion)
  });

  constructor(private dataService: DataService, private router:Router, private formBuilder: FormBuilder) {
    this.dataService.getAllRound1Questions(this.yEvent).subscribe({next: (q => {
      this.possibleAnswers = q;
    }),
    complete: () => {
      this.dataService.getCurrentQuestion(this.yEvent).subscribe(c => {
        this.status = c.status;
        this.currentQuestion = c.questionNum;
        this.selectedQuestion = c.questionNum;
        this.answerForm.patchValue({selectedQuestion: this.selectedQuestion});
      });
    }});
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

    // used to keep SignalR happy and not throwing warnings
    connection.on("round1ShowAnswerChoices", (data: any) => {});

    connection.on("round1OpenAnswer", (data: any) => {});

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

  sendClientMessage(status: number)
  {
    if (status == 0)
    {
      this.selectedQuestion = this.answerForm.value.selectedQuestion;
    }
    console.log(this.yEvent);
    console.log(this.selectedQuestion);
    console.log(status);
    this.dataService.changeRound1QuestionStatus(this.yEvent, this.selectedQuestion, status).subscribe(c => {
      this.status = c.status;
      this.currentQuestion = c.questionNum;
      this.selectedQuestion = c.questionNum;
    });
  }

  sendNextClientMessage(status: number)
  {
    // brute force for now, this should become more elegant
    this.selectedQuestion = this.currentQuestion + 1;
    this.currentQuestion = this.selectedQuestion;
    this.answerForm.patchValue({selectedQuestion: this.selectedQuestion});
    this.sendClientMessage(status);
  }

  updateRemoteScoreboard()
  {
    this.dataService.updateScoreboardDisplay();
  }

  autoScore()
  {
    this.dataService.round1AutoScore(this.yEvent, this.currentQuestion).subscribe(as => {
      this.scoreResponse = as;
    });
    // add loadTeamAnswers as then
  }

  scoreManual(team: number)
  {
    this.dataService.round1ManualScore(this.yEvent, this.currentQuestion, team);
  }

  finalizeRound()
  {
    this.dataService.finalizeRound1(this.yEvent);
  }

  goToRound2()
  {
    this.router.navigate(['/control/round2']);
  }

}
