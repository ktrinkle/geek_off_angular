import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';
import { UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';
import { DataService } from 'src/app/data.service';
import { Store } from '@ngrx/store';
import { selectCurrentEvent, selectAllEvents } from 'src/app/store';
import { eventMaster } from 'src/app/data/data';

@Component({
  selector: 'app-eventchooser',
  templateUrl: './eventchooser.component.html',
  styleUrls: ['./eventchooser.component.scss']
})
export class EventchooserComponent implements OnInit, OnDestroy {

  constructor(private dataService: DataService, private store: Store) {}

  yEvent = '';
  allEvents: eventMaster[] = [];
  showMessage: boolean = false;


  public selectEventForm: UntypedFormGroup = new UntypedFormGroup({
    yEvent: new UntypedFormControl(),
    eventName: new UntypedFormControl(),
    selEvent: new UntypedFormControl()
  });


  destroy$: Subject<boolean> = new Subject<boolean>();

  ngOnInit(): void {
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
    });

    this.reloadEvents();
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

  reloadEvents() {
    this.store.select(selectAllEvents).pipe(takeUntil(this.destroy$)).subscribe(allEvents =>
      this.allEvents = allEvents as eventMaster[]
    );
  }

  setActiveEvent() {
    // uses selectEventForm
    let selYEvent = this.selectEventForm.value.yEvent;
    if (selYEvent)
    {
      var rtnMessage = this.dataService.setCurrentEvent(selYEvent);
    }
  }

}
