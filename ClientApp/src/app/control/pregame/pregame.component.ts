import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';


@Component({
  selector: 'app-pregame',
  templateUrl: './pregame.component.html',
  styleUrls: ['./pregame.component.scss']
})
export class PregameComponent implements OnInit {

  currentPage: string = '1';
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
    teamNum: new FormControl('')
  });

  constructor(private formBuilder: FormBuilder, private router: Router) { }

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

    // we only want to flip automatically if we have to
    connection.on("round1ShowAnswerChoices", (data: any) => {});

  }

  changeScreen(event:any){

  }

  moveToRound1() {
    this.router.navigate(['/control/round1']);
  }

}
