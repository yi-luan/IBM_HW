import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule} from '@angular/material/input';
import { MatIconModule} from '@angular/material/icon';
import { MatButtonModule} from '@angular/material/button';
import { MatToolbarModule} from '@angular/material/toolbar';
import { MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { MatPaginatorIntl } from '@angular/material/paginator';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { FormsModule } from '@angular/forms'; 

import { AllcompanylistComponent } from './components/allHeadquarterCompanylist/allcompanylist.component';
import { CustomPaginator } from 'src/utils/CustomPaginatorConfiguration';
import { LoadingspinnerComponent } from 'src/utils/loadingSpinner/loadingspinner.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DeleteDialogComponent } from './components/deleteDialog/delete-dialog/delete-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { AllbranchcompanylistComponent } from './components/allChildCompanylist/allbranchcompanylist/allbranchcompanylist.component';


@NgModule({
  declarations: [
    AppComponent,
    AllcompanylistComponent,
    LoadingspinnerComponent,
    DeleteDialogComponent,
    AllbranchcompanylistComponent
  ],
  imports: [
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    FormsModule,
    MatFormFieldModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatToolbarModule,
    MatButtonModule,
    MatPaginatorModule,
    MatIconModule,
    HttpClientModule,
    MatProgressSpinnerModule,
    MatDialogModule
  ],
  providers: [
    {provide: MatPaginatorIntl,useValue: CustomPaginator()}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
