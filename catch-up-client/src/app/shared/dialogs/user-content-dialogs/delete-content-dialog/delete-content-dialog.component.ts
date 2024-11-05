import { Component, Inject, OnInit } from '@angular/core';
import { UserProfileHttpService } from '../../../../core/services/http/user-content/user-profile-http.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentHttpService } from '../../../../core/services/http/user-content/comment-http.service';
import { PostHttpService } from '../../../../core/services/http/user-content/post-http.service';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';
import { StoryHttpService } from '../../../../core/services/http/user-content/story-http.service';

@Component({
  selector: 'app-delete-content-dialog',
  templateUrl: './delete-content-dialog.component.html',
  styleUrl: './delete-content-dialog.component.scss'
})
export class DeleteContentDialogComponent implements OnInit {
  public fileName: string;

  constructor(@Inject(MAT_DIALOG_DATA) public data: {
                  userId: string,
                  type: 'cover' | 'profile',
                  imageUrl: string
                  commentId: number;
                  postId: number;
                  storyId: number;
              },
              private userProfileHttpService: UserProfileHttpService,
              private commentHttpService: CommentHttpService,
              private postHttpService: PostHttpService,
              private storyHttpService: StoryHttpService,
              private notificationService: NotificationService) { }

  public ngOnInit(): void {
    if(this.data.commentId == null && this.data.imageUrl) {
      this.fileName = this.data.imageUrl.split('/').pop()!!;
    }
  }

  public get isComment(): boolean {
    return this.data.commentId != null;
  }

  public get isPost(): boolean {
    return this.data.postId != null; 
  }

  public get isProfileOrCover(): boolean {
    return !this.isPost && !this.isComment && !this.isStory;
  }

  public get isStory(): boolean {
    return this.data.storyId != null;
  }

  public onDelete(): void {
    if(this.isProfileOrCover) {
      if(this.data.type === 'cover')
        this.userProfileHttpService.deleteCoverPic(this.data.userId, this.fileName).subscribe();
      else
        this.userProfileHttpService.deleteProfilePic(this.data.userId, this.fileName).subscribe();
    }

    else if(this.isComment) {
      this.commentHttpService.deleteComment(this.data.commentId).subscribe();
    }
    else if(this.isPost) {
      this.postHttpService.delete(this.data.postId).subscribe(_ => {
        this.notificationService.showSuccesSnackBar("Post deleted!");
      })
    }
    else if(this.isStory) {
      this.storyHttpService.delete(this.data.storyId).subscribe(_ => {
        this.notificationService.showSuccesSnackBar("Story deleted!");
      })
    }
  }
}
