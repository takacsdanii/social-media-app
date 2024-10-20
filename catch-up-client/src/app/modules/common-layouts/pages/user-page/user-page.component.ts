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
import { DeleteContentDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/delete-content-dialog/delete-content-dialog.component';
import { EditContentDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/edit-bio-dialog/edit-content-dialog.component';
import { PostHttpService } from '../../../../core/services/http/user-content/post-http.service';
import { VisibilityModel } from '../../../../core/models/enums/visibility.model';
import { UploadPostModel } from '../../../../core/models/user-content/upload-post.model';
import { PostsComponent } from '../../user-content/posts/posts.component';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.scss'
})
export class UserPageComponent implements OnInit {
  public selectedFiles: File[] = [];
  public postDescription: string | null = null;
  public postVisibility: VisibilityModel = 0;

  @ViewChild(NavigationHeaderComponent) navigationHeaderComponent!: NavigationHeaderComponent;
  @ViewChild(RightSideBarComponent) rightSideBarComponent!: RightSideBarComponent;
  @ViewChild(LeftSideBarComponent) leftSideBarComponent!: LeftSideBarComponent;
  @ViewChild(PostsComponent) postsComponent!: PostsComponent;

  public user: UserModel = new UserModel();
  public userIdFromRoute: string | null;
  public following: boolean;

  public isAdmin: boolean;
  private myUserId: string

  public currentImageType: 'profile' | 'cover';

  constructor(private route: ActivatedRoute,
              private authService: AuthService,
              private friendsHttpService: FriendsHttpService,
              private userHttpService: UserHttpService,
              private postHttpService: PostHttpService,
              public mediaUrlService: MediaUrlService,
              private uploadDiaog: MatDialog,
              private displayContentDialog: MatDialog,
              private viewportScroller: ViewportScroller,
              private notificationService: NotificationService) { }

  public ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
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

  public openDisplayContentDialog(type: 'cover' | 'profile'): void {
    const imageUrl = (type === 'cover') ? this.user.coverPicUrl : this.user.profilePicUrl;
    if(type === 'profile') {
      this.displayContentDialog.open(DisplayContentDialogComponent, {
        width: 'auto',
        height: '100%',
        data: { imageUrl }
      });
    }
    else {
      this.displayContentDialog.open(DisplayContentDialogComponent, {
        width: '2000px',
        height: 'auto',
        data: { imageUrl }
      });
    }
  }

  public openDeletePictureDialog(userId: string, type: 'cover' | 'profile') {
    const imageUrl = (type === 'cover') ? this.user.coverPicUrl : this.user.profilePicUrl;
    const dialogref = this.displayContentDialog.open(DeleteContentDialogComponent, {
      height: '250px',
      width: '450px',
      data: { userId, type, imageUrl }
    });

    dialogref.afterClosed().subscribe(result => {
      this.refreshNavbars();
      this.ngOnInit();
    });
  }

  public hasProfilePic(): boolean {
    return this.user.profilePicUrl.split('/').pop() != "female.png"
        && this.user.profilePicUrl.split('/').pop() != "male.png"
        && this.user.profilePicUrl.split('/').pop() != "other.jpg";
  }

  public hasCoverPic(): boolean {
    return this.user?.coverPicUrl?.split('/').pop() != "default-cover.jpg";
  }

  public openEditBioDialog(userId: string, bio: string | null): void {
    const dialogref = this.displayContentDialog.open(EditContentDialogComponent, {
      width: '250px',
      data: { userId, bio }
    });

    dialogref.afterClosed().subscribe(_ => {
      this.ngOnInit();
    });
  }

  public onFilesSelected(event: any): void {
    this.selectedFiles = Array.from(event.target.files);
  }

  public uploadPost(): void {
    if(this.postDescription === '')
      this.postDescription = null;

    var uploadModel: UploadPostModel = {
      userId: this.user.id,
      description: this.postDescription,
      visibility: this.postVisibility
    }

    this.postHttpService.uploadPost(uploadModel, this.selectedFiles).subscribe(post => {
      this.postsComponent.loadPosts();
      this.ngOnInit();
    });
  }

  public canUpload(): boolean {
    return (
      (this.postDescription != null && this.postDescription != '') ||
      this.selectedFiles.length > 0
    );
  }
}