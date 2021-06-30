import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminViewSubscriptionsComponent } from './admin-view-subscriptions.component';

describe('AdminViewSubscriptionsComponent', () => {
  let component: AdminViewSubscriptionsComponent;
  let fixture: ComponentFixture<AdminViewSubscriptionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminViewSubscriptionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminViewSubscriptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
