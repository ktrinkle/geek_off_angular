import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round2hostComponent } from './round2host.component';

describe('Round2hostComponent', () => {
  let component: Round2hostComponent;
  let fixture: ComponentFixture<Round2hostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round2hostComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round2hostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
