import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round2countdownComponent } from './round2countdown.component';

describe('Round2countdownComponent', () => {
  let component: Round2countdownComponent;
  let fixture: ComponentFixture<Round2countdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round2countdownComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round2countdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
