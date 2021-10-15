import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { CountdownConfig  } from 'ngx-countdown';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-round2countdown',
  templateUrl: './round2countdown.component.html',
  styleUrls: ['./round2countdown.component.scss']
})
export class Round2countdownComponent implements OnInit {

  // Need to make this dynamic.
  timeData: number = 0;
  config: CountdownConfig = {};

  constructor() {
   }

  ngOnInit(): void {
    this.config = {
      leftTime: 0,
      demand: true,
      // We need the below to show just the seconds.
      formatDate: ({ date }) => `${date / 1000}`
    };

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.api_url + '/events')
      .build();

    connection.start().then(function () {
      console.log('SignalR Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("startCountdown", (seconds: number) => {
      this.startCountdown(seconds);
    });

    connection.on("stopCountdown", (data: any) => {
      this.stopCountdown();
    });

    connection.on("setCountdown", (seconds: number) => {
      this.timeData = seconds;
      this.config = {
        leftTime: seconds,
        demand: true,
        // We need the below to show just the seconds.
        formatDate: ({ date }) => `${date / 1000}`
      };
    });

  }

  startCountdown(seconds: number){
    this.config = {
      leftTime: seconds,
      demand: false,
      // We need the below to show just the seconds.
      formatDate: ({ date }) => `${date / 1000}`
    };
  }

  stopCountdown(){
    this.config = {
      leftTime: this.timeData,
      demand: true,
      // We need the below to show just the seconds.
      formatDate: ({ date }) => `${date / 1000}`
    };
  }

  handleEvent(event: any){
    console.log(event);
  }

}
