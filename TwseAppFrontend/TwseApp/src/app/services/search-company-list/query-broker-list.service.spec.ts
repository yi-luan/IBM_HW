import { TestBed } from '@angular/core/testing';

import {QueryBrokerListService } from './query-broker-list.service';

describe('SearchCompanyListService', () => {
  let service: QueryBrokerListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QueryBrokerListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
