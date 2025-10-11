import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BigdisplayComponent } from './bigdisplay.component';

describe('BigdisplayComponent', () => {
  let component: BigdisplayComponent;
  let fixture: ComponentFixture<BigdisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BigdisplayComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BigdisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
