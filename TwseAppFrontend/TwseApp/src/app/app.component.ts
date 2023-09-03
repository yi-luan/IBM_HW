import { Component, HostListener } from '@angular/core';
import { EVENT_CLOSE_LOADING, EVENT_SHOW_LOADING } from 'src/utils/loadingSpinner/loadingspinner.component';
import { DeleteDialogComponent } from './components/deleteDialog/delete-dialog/delete-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'TwseApp';
  isLoading = false;
  animal: string = "";
  name: string = "";

  constructor(public dialog: MatDialog){

  }

  @HostListener(`window:${EVENT_SHOW_LOADING}`,['$event'])
  showLoading(){
    this.isLoading = true;
  }

  @HostListener(`window:${EVENT_CLOSE_LOADING}`,['$event'])
  closeLoading(){
    this.isLoading = false;
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {name: this.name, animal: this.animal},
    });

    dialogRef.afterClosed().subscribe((result: string) => {
      console.log('The dialog was closed');
      this.animal = result;
    });
  }
}
