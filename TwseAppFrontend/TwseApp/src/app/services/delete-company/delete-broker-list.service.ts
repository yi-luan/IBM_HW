import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DeleteBrokerListResponse } from 'src/app/models/deleteBrokerListResponse';

@Injectable({
  providedIn: 'root'
})
export class DeleteBrokerListService {

  baseApi:string = "https://localhost:7135";
  constructor(private http:HttpClient) {}

  deleteBrokerList(companiesCode:string): Observable<DeleteBrokerListResponse>{
      return this.http.post<DeleteBrokerListResponse>(this.baseApi + '/api/Twse/DeleteBrokerList',{ companiesCode:companiesCode });
  }
}
