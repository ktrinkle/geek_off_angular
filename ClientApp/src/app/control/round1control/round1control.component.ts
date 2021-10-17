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
  finalizeState: string = 'Finalize round';

  // think cues
  think1 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think1.mp3');
  think2 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think2.mp3');
  think3 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think3.mp3');
  think4 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think4.mp3');
  think5 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think5.mp3');
  think6 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think6.mp3');

  currentFilterQuestion: any = {};

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
        this.currentFilterQuestion = this.possibleAnswers.find(p => p.questionNum == c.questionNum);
      });
      this.updateScoreboard();
      this.loadTeamAnswers();
    }});
  }

  ngOnInit(): void {
    this.think1.load();
    this.think2.load();
    this.think3.load();
    this.think4.load();
    this.think5.load();
    this.think6.load();

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

      // reset form
      this.answerForm = new FormGroup({
        selectedQuestion: new FormControl(this.currentQuestion)
      });

      a.forEach((ans: round1EnteredAnswers) => {
        console.log(a);
        this.answerForm.setControl(ans.teamNum.toString(), new FormControl(ans.answerStatus));
      });

      console.log(this.answerForm);
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
      this.currentFilterQuestion = this.possibleAnswers.find(p => p.questionNum == this.answerForm.value.selectedQuestion);

      // reset answerform
      this.answerForm = new FormGroup({
        selectedQuestion: new FormControl(this.currentQuestion)
      });
    }
    console.log(this.yEvent);
    console.log(this.selectedQuestion);
    console.log(status);
    this.dataService.changeRound1QuestionStatus(this.yEvent, this.selectedQuestion, status).subscribe({next: (c => {
      this.status = c.status;
      this.currentQuestion = c.questionNum;
      this.selectedQuestion = c.questionNum;
      }), complete: () => {
        // drops old answers
        this.loadTeamAnswers();
      }
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
    this.updateScoreboard();
  }

  autoScore()
  {
    this.dataService.round1AutoScore(this.yEvent, this.currentQuestion).subscribe(as => {
      this.scoreResponse = as;
    });
  }

  scoreManual(team: number)
  {
    this.dataService.round1ManualScore(this.yEvent, this.currentQuestion, team);
  }

  finalizeRound()
  {
    this.dataService.finalizeRound1(this.yEvent).subscribe(data => {
      this.finalizeState = data;
    });
  }

  goToRound2()
  {
    this.router.navigate(['/control/round2']);
  }

  playThink(cue: number)
  {
    switch (cue) {
      case 1:
        if (!this.think1.paused && this.think1.currentTime > 0) {
          this.think1.pause;
          this.think1.currentTime =0;
        }
        else
        {
          this.think1.currentTime = 0;
          this.think1.play();
        };
        break;
      case 2:
        if (!this.think2.paused && this.think2.currentTime > 0) {
          this.think2.pause;
          this.think2.currentTime = 0;
        }
        else
        {
          this.think2.currentTime = 0;
          this.think2.play();
        };
        break;
      case 3:
        if (!this.think3.paused && this.think3.currentTime > 0) {
          this.think3.pause;
          this.think3.currentTime = 0;
        }
        else
        {
          this.think3.currentTime = 0;
          this.think3.play();
        };
        break;
      case 4:
        this.think4.play().then( _b => {
          this.think4.currentTime = 0;
          this.think4.pause;
        });
        break;
      case 5:
        this.think5.play().then( _b => {
          this.think5.currentTime = 0;
          this.think5.pause;
        });
        break;
      case 6:
        this.think6.play().then( _b => {
          this.think6.currentTime = 0;
          this.think6.pause;
        });
        break;
    }
  }

  changeScreen(name: string){
    this.dataService.changeIntroPage(name);
  }

}
