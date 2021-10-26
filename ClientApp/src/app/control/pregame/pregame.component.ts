import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { DataService } from 'src/app/data.service';

@Component({
  selector: 'app-pregame',
  templateUrl: './pregame.component.html',
  styleUrls: ['./pregame.component.scss']
})
export class PregameComponent implements OnInit {

  currentPage: string = '1';
  currentPageName: string = 'Intro page 1';
  seatBelt: boolean = false;
  yEvent = sessionStorage.getItem('event') ?? '';
  audio = new Audio();
  seatBeltSound = new Audio();
  listofPages: {key: string, value:string}[] = [
    {key: '1', value: 'Intro page 1'},
    {key: '2', value: 'Intro page 2'},
    {key: 'open', value: 'Seatbelt sign'},
    {key: 'open2', value: 'Intro video'},
    {key: 'teamList', value: 'List of teams'},
    {key: 'rule1', value: 'Rules page 1'},
    {key: 'rule2', value: 'Rules page 2'},
    {key: 'rule3', value: 'Rules page 3'},
  ];

  pageForm: FormGroup = new FormGroup({
    pageName: new FormControl('')
  });

  constructor(private formBuilder: FormBuilder, private router: Router, private dataService: DataService) { }

  ngOnInit(): void {
    this.audio.src = 'https://geekoff2021static.blob.core.windows.net/snd/top_of_hour.mp3';
    this.seatBeltSound.src = 'https://geekoff2021static.blob.core.windows.net/snd/seatbelt.mp3';
    this.audio.load();
    this.seatBeltSound.load();

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

    // we only want to flip automatically if we have to
    connection.on("round1ShowAnswerChoices", (data: any) => {});

  }

  changeScreen(){
    this.dataService.changeIntroPage(this.pageForm.value.pageName);
    this.currentPage = this.pageForm.value.pageName;
  }

  changeSeatBelt() {
    this.seatBelt == false ? true : false;
    this.dataService.changeSeatbelt();
    this.seatBeltSound.play();
  }

  animateText() {
    this.dataService.changeAnimation();
  }

  playCbsSound() {
    this.audio.play();
  }

  moveToRound1() {
    this.dataService.changeRound1QuestionStatus(this.yEvent, 1, 0).subscribe(c => {});
    this.router.navigate(['/control/round1']);
  }

}
