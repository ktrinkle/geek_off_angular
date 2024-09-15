import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventchooserComponent } from './eventchooser.component';

describe('EventchooserComponent', () => {
  let component: EventchooserComponent;
  let fixture: ComponentFixture<EventchooserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventchooserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EventchooserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
