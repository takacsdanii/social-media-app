import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { GenderModel } from '../../../../core/models/enums/gender.model';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.scss'
})
export class UserPageComponent implements OnInit {
  public user: UserModel = new UserModel();
  public userIdFromRoute: string | null;

  constructor(private route: ActivatedRoute,
              private userHttpService: UserHttpService) { }

  public ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.userIdFromRoute = params.get('id');
      if (this.userIdFromRoute) {
        this.setUser(this.userIdFromRoute);
      }
    });
  }

  public setUser(userId: string): void {
    this.userHttpService.getUser(userId).subscribe(_user => {
      this.user = _user;
    });
  }

  public isMypage(userIdFromRoute: string): boolean {
    return this.user.id === userIdFromRoute;
  }

  public getGenderName(gender: number): string {
    return GenderModel[gender];
  }
}
