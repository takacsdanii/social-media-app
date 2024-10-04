import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { UserContentService } from '../../../../core/services/logic/user-conent/user-content.service';

@Component({
  selector: 'app-left-side-bar',
  templateUrl: './left-side-bar.component.html',
  styleUrl: './left-side-bar.component.scss'
})
export class LeftSideBarComponent implements OnInit {
  public user?: UserModel;
  public userId: string;

  constructor(private authService: AuthService,
              private userHttpService: UserHttpService,
              private userContentService: UserContentService) { }

  public ngOnInit(): void {
      this.userId = this.authService.getUserId()!!;
      this.userHttpService.getUser(this.userId).subscribe(user => {
        this.user = user;
      });
  }

  public setProfilePic(user: UserModel): string {
    return this.userContentService.setProfilePic(user);
  }
}
