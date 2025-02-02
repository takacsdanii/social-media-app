import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { GenderModel } from '../../../../core/models/enums/gender.model';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { FriendsHttpService } from '../../../../core/services/http/friends/friends-http.service';
import { RightSideBarComponent } from '../../navigation-bars/right-side-bar/right-side-bar.component';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';
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
import { TimeFormatterService } from '../../../../core/services/logic/helpers/time-formatter.service';
import { StoryHttpService } from '../../../../core/services/http/user-content/story-http.service';
import { StoryDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/story-dialog/story-dialog.component';
import { UploadStoryDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/upload-story-dialog/upload-story-dialog.component';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';
import { FollowersDialogComponent } from '../../../../shared/dialogs/followers-dialog/followers-dialog.component';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.scss'
})
export class UserPageComponent implements OnInit {
  public selectedFiles: File[] = [];
  public postDescription: string | null = null;
  public postVisibility: VisibilityModel = VisibilityModel.Public;

  @ViewChild(NavigationHeaderComponent) navigationHeaderComponent!: NavigationHeaderComponent;
  @ViewChild(RightSideBarComponent) rightSideBarComponent!: RightSideBarComponent;
  @ViewChild(LeftSideBarComponent) leftSideBarComponent!: LeftSideBarComponent;
  @ViewChild(PostsComponent) postsComponent!: PostsComponent;

  public user: UserModel = new UserModel();
  public userIdFromRoute: string | null;
  public following: boolean;
  public isStoryUploaded: boolean;

  public isAdmin: boolean;
  private myUserId: string

  public currentImageType: 'profile' | 'cover';

  constructor(private route: ActivatedRoute,
              private authService: AuthService,
              private friendsHttpService: FriendsHttpService,
              private userHttpService: UserHttpService,
              private postHttpService: PostHttpService,
              private storyHttpService: StoryHttpService,
              private mediaUrlService: MediaUrlService,
              private uploadDialog: MatDialog,
              private uploadStoryDialog: MatDialog,
              private displayContentDialog: MatDialog,
              private followersDialog: MatDialog,
              private viewportScroller: ViewportScroller,
              private timeFormatterService: TimeFormatterService,
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
        this.hasUserUploadedStory(this.userIdFromRoute);
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

  private hasUserUploadedStory(userId: string): void {
    this.storyHttpService.hasUserUploadedVisibleStoryForLoggedInUser(userId, this.myUserId).subscribe(resp => {
      this.isStoryUploaded = resp.result;
    });
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
      this.postsComponent.loadPosts();
      // this.ngOnInit();
      this.setUser(this.userIdFromRoute!);
      this.hasUserUploadedStory(this.userIdFromRoute!);
    });
  }

  public onUnFollowPressed(targetUserId: string): void {
    this.friendsHttpService.unFollowUser(this.myUserId, targetUserId).subscribe(_ => {
      this.checkIfIFollowUser();
      this.rightSideBarComponent.ngOnInit();
      this.postsComponent.loadPosts();
      // this.ngOnInit();
      this.setUser(this.userIdFromRoute!);
      this.hasUserUploadedStory(this.userIdFromRoute!);
    });
  }

  public onFollowUpdated(): void {
    this.setUser(this.userIdFromRoute!);
    this.checkIfIFollowUser();
    this.postsComponent.loadPosts();
  }

  public openUploadDialog(userId: string, type: 'cover' | 'profile'): void {
    if(this.isMypage()) {
      const dialogref = this.uploadDialog.open(UploadDialogComponent, {
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

  public uploadPost(fileInput: HTMLInputElement): void {
    if(this.postDescription === '')
      this.postDescription = null;

    var uploadModel: UploadPostModel = {
      userId: this.user.id,
      description: this.postDescription,
      visibility: this.postVisibility
    }

    this.postHttpService.uploadPost(uploadModel, this.selectedFiles).subscribe(
      post => {
        this.postsComponent.loadPosts();
        this.notificationService.showSuccesSnackBar('Successfully uploaded!');
        this.ngOnInit();
        fileInput.value = '';
        this.postDescription = null;
        this.postVisibility = VisibilityModel.Public;
        this.selectedFiles = [];
      },
      error => {
        if (error.status === 400) {
          this.notificationService.showErrorSnackBar('Upload failed: File size exceeds the limit or invalid data.');
        } else {
          this.notificationService.showErrorSnackBar('An unexpected error occurred. Please try again.');
        }
      }
    );
  }

  public canUpload(): boolean {
    return (
      (this.postDescription != null && this.postDescription != '') ||
      this.selectedFiles.length > 0
    );
  }

  public get timeAgo(): string {
    return this.timeFormatterService.getTimeAgo(this.user.registeredAt);
  }

  public get profilePicUrl(): string | null{
    return this.mediaUrlService.getFullUrl(this.user.profilePicUrl);
  }

  public get coverPicUrl(): string | null {
    return this.mediaUrlService.getFullUrl(this.user.coverPicUrl);
  }

  public openStoryDialog(userId: string): void {
    const dialogRef = this.displayContentDialog.open(StoryDialogComponent, {
      width: '400px',
      height: '100%',
      data: { userId }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.ngOnInit();
    });
  }

  public openUploadStoryDialog(userId: string): void {
    if(this.isMypage()) {
      const dialogRef = this.uploadStoryDialog.open(UploadStoryDialogComponent, {
        height: '250px',
        width: '450px',
        data: { userId }
      });

      dialogRef.afterClosed().subscribe(result => {
        this.ngOnInit();
      });
    }
  }

  public openFollowersDialog(userId: string, isFollowersList: boolean): void {
    this.followersDialog.open(FollowersDialogComponent, {
      width: '150px',
      height: '300px',
      data: { userId, isFollowersList }
    });
  }
}