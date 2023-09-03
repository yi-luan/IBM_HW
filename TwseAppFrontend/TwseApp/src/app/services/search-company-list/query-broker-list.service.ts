import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BranchCompanyList } from 'src/app/models/BranchCompanyList';
import { HeadquartersList } from 'src/app/models/HeadquartersList';
import { queryBrokerListRequest } from 'src/app/models/queryBrokerListRequest';

@Injectable({
  providedIn: 'root'
})
export class QueryBrokerListService {

  baseApi:string = "https://localhost:7135";
  constructor(private http:HttpClient) {}

  getAllBrokerList(request:queryBrokerListRequest): Observable<HeadquartersList>{
      return this.http.post<HeadquartersList>(this.baseApi + '/api/Twse/QueryAllParentsBrokerList', request);
  }

  getAllBranchBrokerList(request:queryBrokerListRequest):Observable<BranchCompanyList>{
    return this.http.post<BranchCompanyList>(this.baseApi + '/api/Twse/QueryAllChildBrokerList', request);
  }

}
