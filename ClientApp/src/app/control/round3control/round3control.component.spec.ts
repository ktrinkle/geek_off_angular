import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round3controlComponent } from './round3control.component';

describe('Round3controlComponent', () => {
  let component: Round3controlComponent;
  let fixture: ComponentFixture<Round3controlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round3controlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round3controlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
