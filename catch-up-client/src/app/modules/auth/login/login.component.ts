import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginModel } from '../../../core/models/auth-models/login.model';
import { AuthHttpService } from '../../../core/services/http/auth/auth-http.service';
import { NotificationService } from '../../../core/services/logic/notifications/notification.service';
import { AuthService } from '../../../core/services/logic/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginModel: LoginModel = new LoginModel();

  constructor(private authHttpService: AuthHttpService,
              private authService: AuthService,
              private notificationService: NotificationService,
              private router: Router) { }

  public ngOnInit(): void { }

  public loginToAccount(): void {
    this.authHttpService.login(this.loginModel).subscribe(
      (next) => {
        localStorage.setItem('token', next.token);

        const username = this.authService.getUserName();
        this.notificationService.showSuccesSnackBar(`${username} logged in succesfully!`);

        const userId = this.authService.getUserId();
        this.router.navigate([`/users/${userId}`]);
      },
      (err) => {
        this.notificationService.showErrorSnackBar('Login failed!');
      }
    );
  }

  public get isEverythingFilled(): boolean {
    return Object.values(this.loginModel).every(value => value != null && value !== '');
  }

  public storeEmailInSessionStorage(): void {
    if(this.loginModel.userNameOrEmail) {
      sessionStorage.setItem('email', this.loginModel.userNameOrEmail);
    }
  }
}
