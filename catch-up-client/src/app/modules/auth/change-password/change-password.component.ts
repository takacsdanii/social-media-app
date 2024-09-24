import { Component, OnInit } from '@angular/core';
import { ChangePasswordModel } from '../../../core/models/auth-models/change-password.model';
import { AuthHttpService } from '../../../core/services/http/auth/auth-http.service';
import { ActivatedRoute } from '@angular/router';
import { UserHttpService } from '../../../core/services/http/user/user-http.service';
import { NotificationService } from '../../../core/services/logic/notifications/notification.service';
import { AuthService } from '../../../core/services/logic/auth/auth.service';


@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent implements OnInit {
  public changePasswordModel: ChangePasswordModel = new ChangePasswordModel();

  public errorMessages: string[] = [];

  constructor(private authHttpService: AuthHttpService,
              private userHttpService: UserHttpService,
              private notificationService: NotificationService,
              private route: ActivatedRoute) { }

  public ngOnInit(): void{
    const routeParams = this.route.snapshot.paramMap;
    const userIdFromRoute = routeParams.get('id');

    if(userIdFromRoute) {
      this.setEmail(userIdFromRoute);
    }
  }

  public setEmail(userId: string): void {
    this.userHttpService.getUser(userId).subscribe(
      (_user) => {
        this.changePasswordModel.id = _user.id;
      },
      (err) => {
        this.notificationService.showErrorSnackBar("There is no user like this");
      }
    );
  }

  public get isEverythingFilled(): boolean {
    return Object.values(this.changePasswordModel).every(value => value != null && value !== '');
  }

  public changePassword(): void {
    this.authHttpService.changePassword(this.changePasswordModel).subscribe(
      (_) => {
        this.notificationService.showSuccesSnackBar('New password set.');
        this.resetValues();
      },
      (err) => {
        if (err.error && err.error.errors) 
          this.errorMessages = err.error.errors.join('; ');
        else
          this.errorMessages = ["Something went wrong!"];
        this.notificationService.showErrorSnackBar(`Password change failed: ${this.errorMessages}`);
      }
    );
  }

  public resetValues(): void {
    this.changePasswordModel.oldPassword = '';
    this.changePasswordModel.newPassword = '';
    this.changePasswordModel.confirmNewPassword = '';
  }
}
