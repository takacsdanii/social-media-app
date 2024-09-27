import { Component, OnInit } from '@angular/core';
import { UserHttpService } from '../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../core/models/user.model';
import { AuthService } from '../../../core/services/logic/auth/auth.service';

@Component({
  selector: 'app-right-side-bar',
  templateUrl: './right-side-bar.component.html',
  styleUrl: './right-side-bar.component.scss'
})
export class RightSideBarComponent implements OnInit {
  public users: UserModel[] = [];
  public loggedInUserId: string;

  constructor(private userHttpService: UserHttpService,
              private authService: AuthService) { }

  public ngOnInit(): void {
    this.loggedInUserId = this.authService.getUserId()!!;
    this.refreshUsers();
  }

  private refreshUsers(): void {
    this.userHttpService.listUsers().subscribe(users => {
      this.users = users.filter(user => user.id != this.loggedInUserId);
    });
  }
}
