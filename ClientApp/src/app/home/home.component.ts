import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { round2AllSurvey } from '../store/round2/round2.actions';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.store.dispatch(round2AllSurvey({ yEvent: 'e21' }))
  }

}
