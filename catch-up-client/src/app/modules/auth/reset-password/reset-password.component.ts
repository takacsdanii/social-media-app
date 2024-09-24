import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from '../../../core/services/http/auth/auth-http.service';
import { ActivatedRoute } from '@angular/router';
import { UserHttpService } from '../../../core/services/http/user/user-http.service';
import { ResetPasswordModel } from '../../../core/models/auth-models/reset-password.model';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent implements OnInit {
  public resetPasswordModel = new ResetPasswordModel();
  public password: string;

  public errorOccurred: boolean;
  public errorMessage: string;

  constructor(private authHttpService: AuthHttpService,
              private userHttpService: UserHttpService,
              private route: ActivatedRoute) { }

  public ngOnInit(): void {
    this.errorOccurred = false;
    const routeParams = this.route.snapshot.paramMap;
    const emailFromRoute = routeParams.get('email');
    const resetTokenFromRoute = routeParams.get('reset-token');

    if(resetTokenFromRoute)
      this.resetPasswordModel.resetToken = resetTokenFromRoute;

    if(emailFromRoute)
      this.setEmail(emailFromRoute);

  }

  public getNewPassword(): void {
    this.authHttpService.resetPassword(this.resetPasswordModel).subscribe(
      (response) => {
        this.password = response.password;
      },
      (err) => {
        this.errorOccurred = true;
        this.errorMessage = "Invaid reset token";
      }
    )
  }

  public setEmail(email: string): void {
    this.userHttpService.getUserByEmail(email).subscribe(
      (_user) => {
        this.resetPasswordModel.email = _user.email;
        this.getNewPassword();
      },
      (err) => {
        this.errorOccurred = true;
        this.errorMessage = "There is no user like this";
      }
    );
  }
}
