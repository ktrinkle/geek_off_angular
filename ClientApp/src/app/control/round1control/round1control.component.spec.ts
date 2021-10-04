import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round1ControlComponent } from './round1control.component';

describe('Round1ControlComponent', () => {
  let component: Round1ControlComponent;
  let fixture: ComponentFixture<Round1ControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round1ControlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round1ControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
