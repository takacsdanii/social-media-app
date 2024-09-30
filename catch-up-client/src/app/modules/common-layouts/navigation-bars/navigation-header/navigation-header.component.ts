import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';

@Component({
  selector: 'app-navigation-header',
  templateUrl: './navigation-header.component.html',
  styleUrl: './navigation-header.component.scss'
})
export class NavigationHeaderComponent implements OnInit {
  public isLoggedIn: boolean = false;
  public isAdmin: boolean = false;
  public userId: string;
  public userName: string;

  public isDarkModeOn: boolean;

  constructor(private authService: AuthService,
              private notificationService: NotificationService) { }

  public ngOnInit(): void {
      this.isLoggedIn = this.authService.isLoggedIn();
      this.isAdmin = this.authService.isAdmin();
      this.userId = this.authService.getUserId()!!;
      this.userName = this.authService.getUserName()!!;

      const storedValue = localStorage.getItem('isDarkModeOn');
      this.isDarkModeOn = storedValue ? JSON.parse(storedValue) : false;
  }

  public logOut(): void {
    this.authService.logOut();
    this.isLoggedIn = false;
    this.notificationService.showSuccesSnackBar("Logged out successfuy!");
  }

  public toggleDarkMode(): void {
    this.isDarkModeOn = !this.isDarkModeOn;
    localStorage.setItem('isDarkModeOn', JSON.stringify(this.isDarkModeOn));
  }
}
