import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { GenderModel } from '../../../../core/models/enums/gender.model';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { FriendsHttpService } from '../../../../core/services/http/friends/friends-http.service';
import { RightSideBarComponent } from '../../navigation-bars/right-side-bar/right-side-bar.component';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.scss'
})
export class UserPageComponent implements OnInit {
  @ViewChild(RightSideBarComponent) rightSideBarComponent!: RightSideBarComponent;

  public user: UserModel = new UserModel();
  public userIdFromRoute: string | null;
  public following: boolean;

  private myUserId: string


  constructor(private route: ActivatedRoute,
              private authService: AuthService,
              private friendsHttpService: FriendsHttpService,
              private userHttpService: UserHttpService) { }

  public ngOnInit(): void {
    this.myUserId = this.authService.getUserId()!!;

    this.route.paramMap.subscribe((params) => {
      this.userIdFromRoute = params.get('id');
      if (this.userIdFromRoute) {
        this.setUser(this.userIdFromRoute);
        this.checkIfIFollowUser();
      }
    });
  }

  public setUser(userId: string): void {
    this.userHttpService.getUser(userId).subscribe(_user => {
      this.user = _user;
    });
  }

  public isMypage(): boolean {
    return this.user.id === this.myUserId;
  }

  public checkIfIFollowUser(): void {
    this.friendsHttpService.doesUserFollowTargetUser(this.myUserId, this.userIdFromRoute!!)
      .subscribe(resp => {
        this.following = resp.result;
      });
  }

  public getGenderName(gender: number): string {
    return GenderModel[gender];
  }

  public setProfilePic(): string {
    var basePath: string = "assets/images/defaults";
    if(this.user.profilePicUrl != null)
      return this.user.profilePicUrl;

    switch(this.user.gender) {
      case(0): return `${basePath}/female.png`;
      case(1): return `${basePath}/male.png`;
      case(2): return `${basePath}/other.jpg`;
    }
    return "";
  }

  public setCoverPic(): string {
    if(this.user.coverPicUrl != null)
      return this.user.coverPicUrl;
    return "assets/images/logos/logo3.png";
  }

  public onFollowPressed(targetUserId: string): void {
    this.friendsHttpService.followUser(this.myUserId, targetUserId).subscribe(_ => {
      this.checkIfIFollowUser();
      this.rightSideBarComponent.ngOnInit();
    });
  }

  public onUnFollowPressed(targetUserId: string): void {
    this.friendsHttpService.unFollowUser(this.myUserId, targetUserId).subscribe(_ => {
      this.checkIfIFollowUser();
      this.rightSideBarComponent.ngOnInit();
    });
  }

  public onFollowUpdated(): void {
    this.setUser(this.userIdFromRoute!!);
    this.checkIfIFollowUser();
  }
}
