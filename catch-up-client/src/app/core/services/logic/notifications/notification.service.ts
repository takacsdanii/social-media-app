import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private snackBar: MatSnackBar) { }

  public showSnackBar(msg: string, cls: string): void {
    this.snackBar.open(msg, "", {
      verticalPosition: "bottom",
      horizontalPosition: "center",
      duration: 3000,
      panelClass: [cls]
    });
  }

  public showSuccesSnackBar(msg: string): void {
    this.showSnackBar(msg, 'snack-success');
  }

  public showErrorSnackBar(msg: string): void {
    this.showSnackBar(msg, 'snack-error');
  }
}
