import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { GenderModel } from '../../../../core/models/enums/gender.model';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { FriendsHttpService } from '../../../../core/services/http/friends/friends-http.service';
import { RightSideBarComponent } from '../../navigation-bars/right-side-bar/right-side-bar.component';
import { MediaUrlService } from '../../../../core/services/logic/media-urls/media-url.service';
import { MatDialog } from '@angular/material/dialog';
import { UploadDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/upload-dialog/upload-dialog.component';
import { LeftSideBarComponent } from '../../navigation-bars/left-side-bar/left-side-bar.component';
import { NavigationHeaderComponent } from '../../navigation-bars/navigation-header/navigation-header.component';
import { DisplayContentDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/display-content-dialog/display-content-dialog.component';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.scss'
})
export class UserPageComponent implements OnInit {
  @ViewChild(NavigationHeaderComponent) navigationHeaderComponent!: NavigationHeaderComponent;
  @ViewChild(RightSideBarComponent) rightSideBarComponent!: RightSideBarComponent;
  @ViewChild(LeftSideBarComponent) leftSideBarComponent!: LeftSideBarComponent;

  public user: UserModel = new UserModel();
  public userIdFromRoute: string | null;
  public following: boolean;

  private myUserId: string


  constructor(private route: ActivatedRoute,
              private authService: AuthService,
              private friendsHttpService: FriendsHttpService,
              private userHttpService: UserHttpService,
              public mediaUrlService: MediaUrlService,
              private uploadDiaog: MatDialog,
              private displayContentDialog: MatDialog,
              private viewportScroller: ViewportScroller) { }

  public ngOnInit(): void {
    this.myUserId = this.authService.getUserId()!!;

    this.route.paramMap.subscribe((params) => {
      this.userIdFromRoute = params.get('id');
      if (this.userIdFromRoute) {
        this.setUser(this.userIdFromRoute);
        this.checkIfIFollowUser();
        this.viewportScroller.scrollToPosition([0, 0]);
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

  public openUploadDialog(userId: string, type: 'cover' | 'profile'): void {
    if(this.isMypage()) {
      const dialogref = this.uploadDiaog.open(UploadDialogComponent, {
        height: '250px',
        width: '450px',
        data: { userId, type }
      });
  
      dialogref.afterClosed().subscribe(result => {
        this.refreshNavbars();
        this.ngOnInit();
      });
    }
  }

  private refreshNavbars(): void {
    this.navigationHeaderComponent.ngOnInit();
    this.leftSideBarComponent.ngOnInit();
    this.rightSideBarComponent.ngOnInit();
  }

  public openDisplayContentDialog(): void {
    const dialogref = this.displayContentDialog.open(DisplayContentDialogComponent, {
      height: '1000px',
      width: '1000px',
    });
  }
}
