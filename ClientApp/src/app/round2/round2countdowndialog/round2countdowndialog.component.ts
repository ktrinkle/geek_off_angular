import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface DialogData {
  countdownValue: number;
}

@Component({
  selector: 'app-round2countdowndialog',
  templateUrl: './round2countdowndialog.component.html',
  styleUrls: ['./round2countdowndialog.component.scss']
})
export class Round2countdowndialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<Round2countdowndialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) { }

  ngOnInit(): void {
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
}
