import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { GenerateCompanyListService } from 'src/app/services/generate-company-list/generate-company-list.service';
import { AllCompanyData, CheckIfDatabaseHaveBeenInitResponse } from 'src/app/models/allCompanyData';
import { UtilsService } from 'src/app/services/utils.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { QueryBrokerListService } from 'src/app/services/search-company-list/query-broker-list.service';
import { queryBrokerListRequest } from 'src/app/models/queryBrokerListRequest';
import { HeadquartersList } from 'src/app/models/HeadquartersList';
import { of, switchMap, throwError } from 'rxjs';


interface Headquarters {
  id: string;
  name: string;
  establishmentDate: string;
  address: string;
  telephone: string;
}

@Component({
  selector: 'app-allcompanylist',
  templateUrl: './allcompanylist.component.html',
  styleUrls: ['./allcompanylist.component.scss']
})

export class AllcompanylistComponent implements OnInit,AfterViewInit {
    currentPage = 1;
    headquarterList: Headquarters[] = [];
    isloading = false;
    totalCount = 0;
    rowsPerPage = 10;
    search_code = new FormControl('', [Validators.required]);
    start_date = new FormControl('');
    end_date = new FormControl('');

    @ViewChild('contentPaginator', { static: true }) contentPaginator!: MatPaginator;

    constructor(
      private allCompanyService:GenerateCompanyListService,
      private queryBrokerListService:QueryBrokerListService
    ){}

    

    ngOnInit():void{
      
      UtilsService.showLoading();

      this.allCompanyService.CheckIfDatabaseHaveBeenInit().subscribe({
        next: (response:CheckIfDatabaseHaveBeenInitResponse) => {
          this.searchParentCompaniesData();
          if(!response.status){
            this.allCompanyService.getAllCompanyData().subscribe({
              next: (response:AllCompanyData) => {
                if (!response.status) {
                  alert("error loading");
                }
                this.searchParentCompaniesData();
              }
            })
          }
        }
      })

      UtilsService.closeLoading();
    }

    ngAfterViewInit() {
      this.contentPaginator.page.subscribe((page: PageEvent) => {
        this.currentPage = page.pageIndex + 1;
        this.searchParentCompaniesData();
      });
    }

    searchParentCompaniesData(){
      const searchRequest:queryBrokerListRequest = {
        start_date:"",
        end_date:"",
        search_code:"",
        current_page:this.currentPage
      }
      
      if (!!this.start_date.value) searchRequest.start_date = new Date(this.start_date.value).toISOString().slice(0,10).replaceAll("-","/");
      if (!!this.end_date.value) searchRequest.end_date = new Date(this.end_date.value).toISOString().slice(0,10).replaceAll("-","/");
      if (!!this.search_code.value) searchRequest.search_code = this.search_code.value;

      this.queryBrokerListService.getAllBrokerList(searchRequest).subscribe({
        next: (response:HeadquartersList) => {
          this.headquarterList = response.headquarters;
          this.totalCount = response.totalNumber;
          UtilsService.closeLoading();
        }
      })
    }

    getErrorMessage() {
      if (this.search_code.hasError('required')) {
        return 'You must enter a value';
      }
  
      return this.search_code.hasError('email') ? 'Not a valid email' : '';
    }
}