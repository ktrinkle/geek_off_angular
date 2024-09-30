import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { takeUntil } from 'rxjs/operators';
import { DataService } from 'src/app/data.service';
import { round1QDisplay, roundCategory } from 'src/app/data/data';
import { environment } from 'src/environments/environment';
import { Store } from '@ngrx/store';
import { selectCurrentEvent, selectRound3BigDisplay, selectRound3Categories } from 'src/app/store';
import { round3BigDisplay, round3Categories } from 'src/app/store/round3/round3.actions';
import * as signalR from '@microsoft/signalr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bigdisplay',
  standalone: true,
  imports: [CommonModule],
  animations: [
    trigger('fadeIn', [
      state('void', style({
        opacity: 0
      })),
      transition('void => *', animate(1000)),
    ]),
    trigger('slideLeft', [
      transition(':enter', [style({ width: 0 }), animate(300)]),
      transition(':leave', [animate(300, style({ width: 0 }))]),
    ]),
  ],
  templateUrl: './bigdisplay.component.html',
  styleUrl: './bigdisplay.component.scss'
})
export class Round3BigdisplayComponent implements OnInit, OnDestroy{
  destroy$: Subject<boolean> = new Subject<boolean>();

  // dependencies
  dataService = inject(DataService);
  store = inject(Store);

  // control and state management
  yEvent = '';
  bigDisplay: round1QDisplay[] = [];
  categoryList: roundCategory[] = [];

  // flags
  introVideoVisible = true;

  boardVisible = false;
  questionVisible = false;
  currentQuestion = 0;
  currentQuestionText = "";
  showMedia = false;

  boardFlyIn = false;
  boardFlyInNumber = 0;

  categoryNumber = 0;
  categoryShow = false;

  finalShow = false;

  // external URIs
  flyInSound = new Audio('https://geekoff2021static.blob.core.windows.net/snd/flyin.mp3');
  finalJepSound = new Audio('https://geekoff2021static.blob.core.windows.net/snd/jepthink.mp3');

  ngOnInit(): void {
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
      if (this.yEvent && this.yEvent.length > 0) {
        this.store.dispatch(round3BigDisplay({ yEvent: this.yEvent }));
        this.store.dispatch(round3Categories({ yEvent: this.yEvent }));
      }
    });

    this.store.select(selectRound3BigDisplay).pipe(takeUntil(this.destroy$)).subscribe(bigDisplay =>
      this.bigDisplay = bigDisplay as round1QDisplay[]
    );

    this.store.select(selectRound3Categories).pipe(takeUntil(this.destroy$)).subscribe(categories => {
      this.categoryList = categories.sort(c => c.subCategoryNum);
    });

    this.flyInSound.load();

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events', { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("round3BigBoard", () => {
      this.boardFlyIn = false;
      this.questionVisible = false;
      this.boardVisible = true;
    });

    connection.on("round3AnswerShow", (data) => {
      this.boardVisible = false;
      // call question method
    });

    connection.on("round3FinalShow", (data) => {
      this.boardVisible = false;
      // call question method
    });

  }

  boardFlyInCounter(): void {
    // not implemented
  }

  showQuestion(question: number): void {
    const currQIndex = this.bigDisplay.findIndex(q => q.questionNum == question);
    this.currentQuestionText = this.bigDisplay[currQIndex].questionText;
    this.bigDisplay[currQIndex].enabled = false;

    // to be implemented, media file
    this.questionVisible = true;
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
