import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round2countdowndialogComponent } from './round2countdowndialog.component';

describe('Round2countdowndialogComponent', () => {
  let component: Round2countdowndialogComponent;
  let fixture: ComponentFixture<Round2countdowndialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round2countdowndialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round2countdowndialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
