import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllbranchcompanylistComponent } from './allbranchcompanylist.component';

describe('AllbranchcompanylistComponent', () => {
  let component: AllbranchcompanylistComponent;
  let fixture: ComponentFixture<AllbranchcompanylistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AllbranchcompanylistComponent]
    });
    fixture = TestBed.createComponent(AllbranchcompanylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
