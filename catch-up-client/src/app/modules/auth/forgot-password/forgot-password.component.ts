import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../../core/services/logic/notifications/notification.service';
import { AuthHttpService } from '../../../core/services/http/auth/auth-http.service';
import { UserHttpService } from '../../../core/services/http/user/user-http.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent implements OnInit {
  public email: string;

  constructor(private authHttpService: AuthHttpService,
              private userHttpService: UserHttpService,
              private notificationService: NotificationService) { }

  public ngOnInit(): void {
    const email = sessionStorage.getItem('email');
    if(email) {
      this.email = email;
      sessionStorage.removeItem('email');
    } 
  }

  public resetPassword(): void {
    this.userHttpService.getUserByEmail(this.email).subscribe(
      (_) => {
        this.notificationService.showSuccesSnackBar(`Email sent to ${this.email}`);
        this.authHttpService.requestNewPassword(this.email).subscribe();
      },
      (err) => {
        this.notificationService.showErrorSnackBar("There is no user with this email address");
      }
    );
  }

}
