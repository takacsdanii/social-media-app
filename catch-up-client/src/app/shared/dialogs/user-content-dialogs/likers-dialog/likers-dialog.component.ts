import { Component, Inject, OnInit } from '@angular/core';
import { LikeModel } from '../../../../core/models/user-content/like.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LoginModel } from '../../../../core/models/auth-models/login.model';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';
import { LikeHttpService } from '../../../../core/services/http/user-content/like-http.service';

@Component({
  selector: 'app-likers-dialog',
  templateUrl: './likers-dialog.component.html',
  styleUrl: './likers-dialog.component.scss'
})
export class LikersDialogComponent implements OnInit {
  public likers: LikeModel[];

  constructor(private mediaUrlService: MediaUrlService,
              private likeHttpService: LikeHttpService,
              private dialogRef: MatDialogRef<LikersDialogComponent>,
              @Inject(MAT_DIALOG_DATA) private data: { postId: number, commentId: number }) { }

  public ngOnInit(): void {
    if(!this.data.commentId) {
      this.getLikersForPost();
    }
    else {
      this.getLikersForComment();
    }
  }

  private getLikersForPost(): void {
    this.likeHttpService.getLikersForPost(this.data.postId).subscribe(likers => {
      this.likers = likers;
    });
  }

  private getLikersForComment(): void {
    this.likeHttpService.getLikersForComment(this.data.postId, this.data.commentId)
      .subscribe(likers => {
        this.likers = likers;
      });
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }

  public getProfilePicUrl(liker: LikeModel): string | null {
    return liker ? this.mediaUrlService.getFullUrl(liker.profilePicUrl) : null;
  }
}
