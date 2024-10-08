import { Component, OnDestroy, OnInit } from '@angular/core';
import { DataService } from 'src/app/data.service';
import { round1QuestionControlDto, round1Scores, round1EnteredAnswers } from 'src/app/data/data';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { UntypedFormGroup, UntypedFormControl, UntypedFormBuilder } from '@angular/forms';
import { selectCurrentEvent } from 'src/app/store';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-round1',
  templateUrl: './round1control.component.html',
  styleUrls: ['./round1control.component.scss']
})
export class Round1ControlComponent implements OnInit, OnDestroy {

  currentQuestion = 0;
  selectedQuestion = 0;
  possibleAnswers: round1QuestionControlDto[] = [];
  teamAnswers: round1EnteredAnswers[] = [];
  scoreboard: round1Scores[] = [];
  yEvent = '';
  statusEnum: string[] = [''];
  status = 0;
  scoreResponse = '';
  finalizeState = 'Finalize round';
  statusText = '';
  destroy$: Subject<boolean> = new Subject<boolean>();

  // think cues
  think1 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think1.mp3');
  think2 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think2.mp3');
  think3 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think3.mp3');
  think4 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think4.mp3');
  think5 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think5.mp3');
  think6 = new Audio('https://geekoff2021static.blob.core.windows.net/snd/think6.mp3');
  consolation = new Audio('https://geekoff2021static.blob.core.windows.net/snd/r1_consolation.m4a');

  currentFilterQuestion: any;

  public answerForm: UntypedFormGroup = new UntypedFormGroup({
    selectedQuestion: new UntypedFormControl(this.currentQuestion)
  });

  constructor(private dataService: DataService, private router: Router, private formBuilder: UntypedFormBuilder, private store: Store) {

    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.dataService.getAllRound1Questions(this.yEvent).subscribe({
          next: (q => {
            this.possibleAnswers = q;
          }),
          complete: () => {
            this.possibleAnswers.sort(a => a.questionNum);
            this.dataService.getCurrentQuestion(this.yEvent).subscribe(c => {
              this.status = c.status;
              this.currentQuestion = c.questionNum;
              this.selectedQuestion = c.questionNum;
              this.answerForm.patchValue({ selectedQuestion: this.selectedQuestion });
              this.currentFilterQuestion = this.possibleAnswers.find(p => p.questionNum == c.questionNum) ?? {};
            });
            this.updateScoreboard();
            this.loadTeamAnswers();
          }
        });
      }
    });
  }

  ngOnInit(): void {
    this.think1.load();
    this.think2.load();
    this.think3.load();
    this.think4.load();
    this.think5.load();
    this.think6.load();
    this.consolation.load();

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Debug)
      .withUrl(environment.api_url + '/events', { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round1PlayerAnswer", () => {
      this.loadTeamAnswers();
    });

    connection.on("round1ScoreUpdate", () => {
      this.updateScoreboard();
    })

    // connection.on("round1CloseAnswer", (data: any) => {});
  }

  loadTeamAnswers() {
    this.dataService.getAllEnteredAnswers(this.yEvent, this.currentQuestion).subscribe(a => {
      this.teamAnswers = a;

      // reset form
      this.answerForm = new UntypedFormGroup({
        selectedQuestion: new UntypedFormControl(this.currentQuestion)
      });

      a.forEach((ans: round1EnteredAnswers) => {
        this.answerForm.setControl(ans.teamNum.toString(), new UntypedFormControl(ans.answerStatus));
      });
    });
  }

  updateScoreboard() {
    this.dataService.getRound1Scores(this.yEvent).subscribe({
      next: (a => {
        this.scoreboard = a;
      }), complete: () => {
        this.scoreboard.sort((a, b) => 0 - ((a.teamScore ?? 0) > (b.teamScore ?? 0) ? 1 : -1));
      }
    });
  }

  resyncStatus() {
    this.dataService.changeRound1QuestionStatus(this.yEvent, this.selectedQuestion, this.status).subscribe();
  }

  sendClientMessage(status: number) {
    if (status == 0) {
      this.selectedQuestion = this.answerForm.value.selectedQuestion;
      this.currentFilterQuestion = this.possibleAnswers.find(p => p.questionNum == this.answerForm.value.selectedQuestion) ?? {};
      this.scoreResponse = '';

      // reset answerform
      this.answerForm = new UntypedFormGroup({
        selectedQuestion: new UntypedFormControl(this.currentQuestion)
      });
    }
    console.log(this.yEvent);
    console.log(this.selectedQuestion);
    console.log(status);
    this.dataService.changeRound1QuestionStatus(this.yEvent, this.selectedQuestion, status).subscribe({
      next: (c => {
        this.status = c.status;
        this.currentQuestion = c.questionNum;
        this.selectedQuestion = c.questionNum;

        if (status == 0) {
          this.statusText = 'Question displayed';
        }
        if (status == 1) {
          this.statusText = 'Answers displayed';
        }
        if (status == 2) {
          this.statusText = 'Answers open';
        }
        if (status == 3) {
          this.statusText = 'Correct answer displayed';
        }
      }), complete: () => {
        // drops old answers
        this.loadTeamAnswers();
      }
    });
  }

  sendNextClientMessage(status: number) {
    // brute force for now, this should become more elegant
    this.selectedQuestion = this.currentQuestion + 1;
    if (!this.possibleAnswers.find(p => p.questionNum == this.answerForm.value.selectedQuestion)) {
      this.selectedQuestion = 1;
    }
    this.currentQuestion = this.selectedQuestion;
    this.answerForm.patchValue({ selectedQuestion: this.selectedQuestion });
    this.sendClientMessage(status);
  }

  updateRemoteScoreboard() {
    this.dataService.updateScoreboardDisplay();
    // this actually fires twice
    this.updateScoreboard();
  }

  autoScore() {
    this.dataService.round1AutoScore(this.yEvent, this.currentQuestion).subscribe(as => {
      this.scoreResponse = as;
    });
  }

  scoreManual(team: number) {
    this.dataService.round1ManualScore(this.yEvent, this.currentQuestion, team);
  }

  finalizeRound() {
    this.dataService.finalizeRound1(this.yEvent).subscribe(data => {
      this.finalizeState = data;
    });
  }

  goToRound2() {
    this.consolation.pause();
    this.router.navigate(['/control/round2feud']);
  }

  playThink(cue: number) {
    switch (cue) {
      case 1:
        if (!this.think1.paused && this.think1.currentTime > 0) {
          this.think1.pause();
          this.think1.currentTime = 0;
        }
        else {
          this.think1.currentTime = 0;
          this.think1.play();
        }
        break;
      case 2:
        if (!this.think2.paused && this.think2.currentTime > 0) {
          this.think2.pause();
          this.think2.currentTime = 0;
        }
        else {
          this.think2.currentTime = 0;
          this.think2.play();
        }
        break;
      case 3:
        if (!this.think3.paused && this.think3.currentTime > 0) {
          this.think3.pause();
          this.think3.currentTime = 0;
        }
        else {
          this.think3.currentTime = 0;
          this.think3.play();
        }
        break;
      case 4:
        if (!this.think4.paused && this.think4.currentTime > 0) {
          this.think4.pause();
          this.think4.currentTime = 0;
        }
        else {
          this.think4.currentTime = 0;
          this.think4.play();
        }
        break;
      case 5:
        if (!this.think5.paused && this.think5.currentTime > 0) {
          this.think5.pause();
          this.think5.currentTime = 0;
        }
        else {
          this.think5.currentTime = 0;
          this.think5.play();
        }
        break;
      case 6:
        if (!this.think6.paused && this.think6.currentTime > 0) {
          this.think6.pause();
          this.think6.currentTime = 0;
        }
        else {
          this.think6.currentTime = 0;
          this.think6.play();
        }
        break;
    }
  }

  changeScreen(name: string) {
    this.dataService.changeIntroPage(name);
    if (name == 'prize1') {
      this.consolation.play();
    }
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

}
