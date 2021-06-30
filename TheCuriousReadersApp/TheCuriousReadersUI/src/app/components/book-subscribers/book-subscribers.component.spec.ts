import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookSubscribersComponent } from './book-subscribers.component';

describe('BookSubscribersComponent', () => {
  let component: BookSubscribersComponent;
  let fixture: ComponentFixture<BookSubscribersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookSubscribersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookSubscribersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
