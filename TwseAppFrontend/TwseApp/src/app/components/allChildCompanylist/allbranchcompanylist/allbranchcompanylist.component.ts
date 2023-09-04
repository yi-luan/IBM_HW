import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { UtilsService } from 'src/app/services/utils.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { QueryBrokerListService } from 'src/app/services/search-company-list/query-broker-list.service';
import { queryBrokerListRequest } from 'src/app/models/queryBrokerListRequest';
import { BranchCompanyList } from 'src/app/models/BranchCompanyList';
import { GenerateCompanyListService } from 'src/app/services/generate-company-list/generate-company-list.service';
import { AllCompanyData, CheckIfDatabaseHaveBeenInitResponse } from 'src/app/models/allCompanyData';
import { of, switchMap } from 'rxjs';

interface BranchCompany {
  id: number;
  dateOfPublication: string;
  securitiesFirmCode: string;
  securitiesFirmName: string;
  openingDate: string;
  address: string;
  telephone: string;
  code: string;
}

@Component({
  selector: 'app-allbranchcompanylist',
  templateUrl: './allbranchcompanylist.component.html',
  styleUrls: ['./allbranchcompanylist.component.scss']
})

export class AllbranchcompanylistComponent {
    currentPage = 1;
    branchCompanyList: BranchCompany[] = [];
    isloading = false;
    totalCount = 0;
    rowsPerPage = 10;
    search_code = new FormControl('');
    start_date = new FormControl('');
    end_date = new FormControl('');

    @ViewChild('contentPaginator', { static: true }) contentPaginator!: MatPaginator;

    constructor(private queryBrokerListService:QueryBrokerListService,private allCompanyService:GenerateCompanyListService){}

    ngOnInit():void{
      UtilsService.showLoading();
      this.allCompanyService.CheckIfDatabaseHaveBeenInit().pipe(
          switchMap((response: CheckIfDatabaseHaveBeenInitResponse) => {
              if(!response.status) return this.allCompanyService.getAllCompanyData();
              else return of(null)
          })
      )
      .subscribe((response: AllCompanyData | null) => {
          if(response?.status) alert("error");
          this.searchBranchCompaniesData();
          UtilsService.closeLoading();
      });
    }

    ngAfterViewInit() {
      this.contentPaginator.page.subscribe((page: PageEvent) => {
        this.currentPage = page.pageIndex + 1;
        this.searchBranchCompaniesData();
      });
    }

    searchBranchCompaniesData(){
      const searchRequest:queryBrokerListRequest = {
        start_date:"",
        end_date:"",
        search_code:"",
        current_page:this.currentPage
      }
      
      if (!!this.start_date.value) searchRequest.start_date = new Date(this.start_date.value).toISOString().slice(0,10).replaceAll("-","/");
      if (!!this.end_date.value) searchRequest.end_date = new Date(this.end_date.value).toISOString().slice(0,10).replaceAll("-","/");
      if (!!this.search_code.value) searchRequest.search_code = this.search_code.value;

      UtilsService.showLoading();
      this.queryBrokerListService.getAllBranchBrokerList(searchRequest).subscribe({
        next: (response:BranchCompanyList) => {
          this.branchCompanyList = response.branchCompanies;
          this.totalCount = response.totalNumber;
        }
      })
      UtilsService.closeLoading();
    }

    getErrorMessage() {
      if (this.search_code.hasError('required')) {
        return 'You must enter a value';
      }

      return this.search_code.hasError('email') ? 'Not a valid email' : '';
    }
}
