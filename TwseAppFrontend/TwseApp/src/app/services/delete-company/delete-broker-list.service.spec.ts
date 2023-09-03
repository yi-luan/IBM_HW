import { TestBed } from '@angular/core/testing';

import { DeleteBrokerListService } from './delete-broker-list.service';

describe('DeleteBrokerListService', () => {
  let service: DeleteBrokerListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteBrokerListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
