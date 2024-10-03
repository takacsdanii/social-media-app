import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterModel } from '../../../core/models/auth-models/register.model';
import { months } from '../../../core/constants/monts.model';
import { AuthHttpService } from '../../../core/services/http/auth/auth-http.service';
import { NotificationService } from '../../../core/services/logic/notifications/notification.service';
import { AuthService } from '../../../core/services/logic/auth/auth.service';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  public registerModel: RegisterModel = new RegisterModel();

  public years: number[] = [];
  public months = months;
  public days: number[] = [];

  public year: number;
  public month: number;
  public day?: number;

  public errorMessages: string[] = [];

  public get isEverythingFilled(): boolean {
    return Object.values(this.registerModel).every(value => value != null && value !== ''
      && (value instanceof Date ? !isNaN(value.getTime()) : true)
    );
  }

  constructor(private authHttpService: AuthHttpService,
              private authService: AuthService,
              private notificationService: NotificationService,
              private router: Router) { }

  public ngOnInit(): void {
    this.initYears();
  }

  public registerUser(): void {
    this.authHttpService.register(this.registerModel).pipe(
      switchMap(() => {
        this.notificationService.showSuccesSnackBar(`${this.registerModel.userName} registered!`);

        const loginModel = {
          userNameOrEmail: this.registerModel.userName,
          password: this.registerModel.password
        };
        return this.authHttpService.login(loginModel);
      })
    ).subscribe(
      (next) => {
        localStorage.setItem('token', next.token);

        const userId = this.authService.getUserId();
        this.router.navigate([`/home-page/${userId}`]);
      },
      (err) => {
        this.errorMessages = [];
        if (err.error && err.error.errors)
          this.errorMessages = err.error.errors.join('; ');
        else
          this.errorMessages = ["Something went wrong!"];

        this.notificationService.showErrorSnackBar(`Registration failed: ${this.errorMessages}`);
      }
    );
  }

  public onDateChange(): void {
    if (this.year && this.month)
      this.initDays();
    this.registerModel.birthDate = new Date(Date.UTC(this.year, this.month - 1, this.day));
  }

  public initDays(): void {
    this.days = [];
    const daysInMonth = new Date(this.year, this.month, 0).getDate();
    if(this.day && this.day > daysInMonth)
      this.day = undefined;
    for(let i = 1; i <= daysInMonth; i++)
      this.days.push(i);
  }

  public initYears(): void {
    let currentYear = new Date().getFullYear();
    for(let i = 0; i < 120; i++)
      this.years.push(currentYear--);
  }
}
