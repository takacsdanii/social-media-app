import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { SearchUserModel } from '../../../../core/models/search-user.model';
import { UserModel } from '../../../../core/models/user.model';

@Component({
  selector: 'app-navigation-header',
  templateUrl: './navigation-header.component.html',
  styleUrl: './navigation-header.component.scss'
})
export class NavigationHeaderComponent implements OnInit {
  public isLoggedIn: boolean = false;
  public user: UserModel;
  public isAdmin: boolean = false;
  public userId: string;

  public isDarkModeOn: boolean;

  public searchString: string;
  public filteredUsers: SearchUserModel[] = [];

  constructor(private authService: AuthService,
              private userHttpService: UserHttpService,
              private notificationService: NotificationService) { }

  public ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.isAdmin = this.authService.isAdmin();
    this.userId = this.authService.getUserId()!!;

    this.userHttpService.getUser(this.userId).subscribe(user => {
      this.user = user;
    });

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

  public onSearchChange(): void {
    if(this.searchString != "") {
      this.userHttpService.searchUsers(this.searchString).subscribe(users => {
        this.filteredUsers = users;
      });
    }
  }

}
