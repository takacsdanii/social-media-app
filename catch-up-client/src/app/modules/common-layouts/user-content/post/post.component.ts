import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { PostHttpService } from '../../../../core/services/http/user-content/post-http.service';
import { PostModel } from '../../../../core/models/user-content/post.model';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { MediaUrlService } from '../../../../core/services/logic/media-urls/media-url.service';
import { MatDialog } from '@angular/material/dialog';
import { DisplayContentDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/display-content-dialog/display-content-dialog.component';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { EditContentDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/edit-bio-dialog/edit-content-dialog.component';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent implements OnInit {
  public isLiked: boolean;
  public isCommentSectionOpen: boolean = false;
  public currentImgIdx: number = 0;

  constructor(private postHttpService: PostHttpService,
              private userHttpService: UserHttpService,
              public mediaUrlService: MediaUrlService,
              private displayContentDialog: MatDialog,
              private editDialog: MatDialog,
              private authService: AuthService,
              private notificationService: NotificationService) { }

  @Input() public postId: number;
  @Output() public postDeleted: EventEmitter<void> = new EventEmitter<void>();

  public post?: PostModel;
  public user?: UserModel;

  public loggedInUserId: string;
  public isAdmin: boolean;

  public ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
    this.loggedInUserId = this.authService.getUserId()!!;

    this.postHttpService.getPost(this.postId).subscribe(result => {
      this.post = result;
      this.userHttpService.getUser(this.post.userId).subscribe(u => {
        this.user = u;
      })
    });
  }

  public toggleLike(): void {
    this.isLiked = ! this.isLiked;
  }

  public toggleOpenCommentSection(): void {
    this.isCommentSectionOpen = !this.isCommentSectionOpen;
  }

  public prevImage(): void {
    if(this.currentImgIdx > 0)
      this.currentImgIdx--;
  }

  public nextImage(): void {
    if(this.currentImgIdx < this.post!!.mediaUrls.length - 1)
      this.currentImgIdx++;
  }

  public openDisplayContentDialog(imageUrl: string): void {
    this.displayContentDialog.open(DisplayContentDialogComponent, {
      width: 'auto',
      height: '100%',
      data: { imageUrl }
    });
  }

  public isLoggedInUsersPost(): boolean {
    return this.loggedInUserId == this.post?.userId;
  }

  public openEditDialog(postId: number, description: string | null): void {
    const dialogRef =this.editDialog.open(EditContentDialogComponent, {
      width: '250px',
      data: { postId, description }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.ngOnInit();
    })
  }

  public deletePost(postId: number): void {
    this.postHttpService.delete(postId).subscribe(_ => {
      this.notificationService.showSuccesSnackBar("Post deleted!");
      this.postDeleted.emit();
    })
  }
}
