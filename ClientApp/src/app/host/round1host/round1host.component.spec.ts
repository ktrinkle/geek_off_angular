import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Round1hostComponent } from './round1host.component';

describe('Round1hostComponent', () => {
  let component: Round1hostComponent;
  let fixture: ComponentFixture<Round1hostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Round1hostComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Round1hostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
