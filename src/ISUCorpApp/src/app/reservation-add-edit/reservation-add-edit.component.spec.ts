import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationAddEditComponent } from './reservation-add-edit.component';

describe('ReservationAddEditComponent', () => {
  let component: ReservationAddEditComponent;
  let fixture: ComponentFixture<ReservationAddEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReservationAddEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReservationAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
