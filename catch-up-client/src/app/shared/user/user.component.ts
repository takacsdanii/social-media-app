import { Component, OnInit } from '@angular/core';
import { UserModel } from '../../core/models/user.model';
import { UserHttpService } from '../../core/services/http/user/user-http.service';
import { NotificationService } from '../../core/services/logic/notifications/notification.service';
import { AuthService } from '../../core/services/logic/auth/auth.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit{
  public users: UserModel[] = [];

  constructor(private userHttpService: UserHttpService) { }

  public ngOnInit(): void {
    this.getUsers();
  }

  public getUsers(): void {
    this.userHttpService.listUsers().subscribe(_users => {
      this.users = _users;
    });
  }

}
