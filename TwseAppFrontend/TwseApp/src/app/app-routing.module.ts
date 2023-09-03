import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllcompanylistComponent } from './components/allHeadquarterCompanylist/allcompanylist.component';
import { AllbranchcompanylistComponent } from './components/allChildCompanylist/allbranchcompanylist/allbranchcompanylist.component';

const routes: Routes = [
  {
    path: '',
    component: AllcompanylistComponent
  },
  {
    path: 'allBranchCompanyList',
    component: AllbranchcompanylistComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
