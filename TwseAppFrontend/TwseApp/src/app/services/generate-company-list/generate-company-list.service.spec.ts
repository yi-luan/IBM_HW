import { TestBed } from '@angular/core/testing';

import { GenerateCompanyListService } from './generate-company-list.service';

describe('GenerateCompanyListService', () => {
  let service: GenerateCompanyListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GenerateCompanyListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
