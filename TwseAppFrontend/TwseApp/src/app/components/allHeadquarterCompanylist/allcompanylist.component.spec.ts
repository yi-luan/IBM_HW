import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllcompanylistComponent } from './allcompanylist.component';

describe('AllcompanylistComponent', () => {
  let component: AllcompanylistComponent;
  let fixture: ComponentFixture<AllcompanylistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AllcompanylistComponent]
    });
    fixture = TestBed.createComponent(AllcompanylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
