import { Component, Inject, OnInit } from '@angular/core';
import { UserHttpService } from '../../../core/services/http/user/user-http.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthService } from '../../../core/services/logic/auth/auth.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../../core/services/logic/notifications/notification.service';

@Component({
  selector: 'app-delete-user-dialog',
  templateUrl: './delete-user-dialog.component.html',
  styleUrl: './delete-user-dialog.component.scss'
})
export class DeleteUserDialogComponent implements OnInit {
  public isAdmin: boolean = false;

  constructor(private userHttpService: UserHttpService,
              private authService: AuthService,
              private router: Router,
              private notificationService: NotificationService,
              @Inject(MAT_DIALOG_DATA) public data: { userId: string, userName: string }) { }

  public ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
  }

  public onDelete(id: string): void {
    this.userHttpService.deleteUser(id).subscribe(_ => {
      if(!this.isAdmin) {
        this.authService.logOut();
        this.router.navigate(['']);
      }
      else {
        this.router.navigate(['/users']);
      }
      this.notificationService.showSuccesSnackBar("Deleted succesfully");
    });
  }
}
