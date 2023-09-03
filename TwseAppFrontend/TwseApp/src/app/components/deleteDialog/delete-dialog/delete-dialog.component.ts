import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import {MatSnackBar} from '@angular/material/snack-bar';
import { MatDialogRef } from '@angular/material/dialog';
import { DeleteBrokerListResponse } from 'src/app/models/deleteBrokerListResponse';
import { DeleteBrokerListService } from 'src/app/services/delete-company/delete-broker-list.service';


export interface DialogData {
  animal: string;
  name: string;
}

@Component({
  selector: 'app-delete-dialog',
  templateUrl: './delete-dialog.component.html',
  styleUrls: ['./delete-dialog.component.scss']
})

export class DeleteDialogComponent {

  delete_code = new FormControl('', [Validators.required]);
  constructor(
    public dialogRef: MatDialogRef<DeleteDialogComponent>
    ,private deleteBrokerListService:DeleteBrokerListService
    ,private _snackBar: MatSnackBar) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  TaskExecute():void {
    if (!this.delete_code.value || this.delete_code.value === "") return;

    this.deleteBrokerListService.deleteBrokerList(this.delete_code.value).subscribe({
      next:(response:DeleteBrokerListResponse) => {
        if (response.status) {
          this._snackBar.open('Delete Success', 'Info', {
            duration: 3000
          });
        }
        else{
          this._snackBar.open('Delete Failed', 'Info', {
            duration: 3000
          });
        }
      }
    })
    
    this.dialogRef.close();
  }
}
