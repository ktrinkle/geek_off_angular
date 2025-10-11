import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round2controlComponent } from './round2control.component';

describe('Round2controlComponent', () => {
  let component: Round2controlComponent;
  let fixture: ComponentFixture<Round2controlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round2controlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round2controlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
