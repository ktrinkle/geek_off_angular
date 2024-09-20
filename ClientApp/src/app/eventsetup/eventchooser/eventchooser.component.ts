import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DataService } from 'src/app/data.service';
import { Store } from '@ngrx/store';
import { selectCurrentEvent } from 'src/app/store';
import { eventMaster } from 'src/app/data/data';
import { allEvent, currentEvent } from 'src/app/store/eventManage/eventManage.actions';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-eventchooser',
  templateUrl: './eventchooser.component.html',
  styleUrls: ['./eventchooser.component.scss']
})
export class EventchooserComponent implements OnInit, OnDestroy {

  constructor(private dataService: DataService, private store: Store, private snackbar: MatSnackBar) {}

  yEvent = '';
  allEvents: eventMaster[] = [];

  public selectEventForm: FormGroup = new FormGroup({
    yEvent: new FormControl<string>('')
  });

  public newEventForm: FormGroup = new FormGroup({
    yEvent: new FormControl<string>('', Validators.required),
    eventName: new FormControl<string>('', Validators.required)
  });

  destroy$: Subject<boolean> = new Subject<boolean>();

  ngOnInit(): void {
    this.reloadEvents();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }

  reloadEvents(): void {
    this.dataService.getAllEvents().subscribe(data => {
      this.allEvents = data;
      this.getCurrentEvent();
      this.selectEventForm.patchValue({yEvent: this.yEvent});
    });
  }

  createNewEvent(): void {
    if (this.newEventForm.valid)
    {
      const newEvent: eventMaster = {
        yevent: this.newEventForm.value.yEvent,
        eventName: this.newEventForm.value.eventName,
        selEvent: false
      };

      this.dataService.addNewEvent(newEvent).subscribe({
        next: (data) => {
          this.snackbar.open(data, '', {
            duration: 5000
          });
        },
        complete: () => {
          this.reloadEvents();
        }
      });
    }
  }

  setActiveEvent(): void {
    console.log(this.selectEventForm);
    const selYEvent = this.selectEventForm.value.yEvent;
    if (selYEvent)
    {
      this.dataService.setCurrentEvent(selYEvent).subscribe(data => {
        this.snackbar.open(data, '', {
          duration: 5000
        });
        this.yEvent = selYEvent;
        this.store.dispatch(currentEvent());
      });
    }
  }

  getCurrentEvent(): void {
    this.store.select(selectCurrentEvent).pipe(takeUntil(this.destroy$)).subscribe(currentEvent => {
      this.yEvent = currentEvent;
    });
  }

}
