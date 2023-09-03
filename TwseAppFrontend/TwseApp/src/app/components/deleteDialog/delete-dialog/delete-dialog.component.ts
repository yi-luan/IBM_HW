import { Component, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
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

  delete_code = new FormControl('');
  constructor(public dialogRef: MatDialogRef<DeleteDialogComponent>,private deleteBrokerListService:DeleteBrokerListService) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  TaskExecute():void {
    if (!this.delete_code.value) return;

    this.deleteBrokerListService.deleteBrokerList(this.delete_code.value).subscribe({
      next:(response:DeleteBrokerListResponse) => {
        console.log(response);
        if (response.status) alert("Success");
      }
    })
    
    this.dialogRef.close();
  }
}
