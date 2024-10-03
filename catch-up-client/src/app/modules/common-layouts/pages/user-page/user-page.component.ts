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

  public setProfilePic(): string {
    var basePath: string = "assets/images/standard";
    if(this.user.profilePicUrl != null)
      return this.user.profilePicUrl;

    switch(this.user.gender) {
      case(0): return `${basePath}/woman.png`;
      case(1): return `${basePath}/man.jpg`;
      case(2): return `${basePath}/dog.png`;
    }
    return "";
  }

  public setCoverPic(): string {
    if(this.user.coverPicUrl != null)
      return this.user.coverPicUrl;
    return "assets/images/logos/logo3.png";
  }
}
