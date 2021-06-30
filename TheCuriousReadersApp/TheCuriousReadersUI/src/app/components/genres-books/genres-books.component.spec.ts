import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenresBooksComponent } from './genres-books.component';

describe('GenresBooksComponent', () => {
  let component: GenresBooksComponent;
  let fixture: ComponentFixture<GenresBooksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenresBooksComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenresBooksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
