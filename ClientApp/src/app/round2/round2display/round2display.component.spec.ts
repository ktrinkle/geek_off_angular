import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round2displayComponent } from './round2display.component';

describe('Round2displayComponent', () => {
  let component: Round2displayComponent;
  let fixture: ComponentFixture<Round2displayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round2displayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round2displayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
