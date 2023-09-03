import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AllCompanyData, CheckIfDatabaseHaveBeenInitResponse } from '../../models/allCompanyData';

@Injectable({
  providedIn: 'root'
})
export class GenerateCompanyListService {
  baseApi:string = "https://localhost:7135";
  constructor(private http:HttpClient) {}

  getAllCompanyData(): Observable<AllCompanyData>{
      return this.http.post<AllCompanyData>(this.baseApi + '/api/Twse/InitializeTwseCompanyData',{});
  }

  CheckIfDatabaseHaveBeenInit(): Observable<CheckIfDatabaseHaveBeenInitResponse>{
    return this.http.post<CheckIfDatabaseHaveBeenInitResponse>(this.baseApi + '/api/Twse/CheckIfDatabaseHaveBeenInit',{});
  }
}
