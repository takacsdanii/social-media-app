import { Component, Inject, OnInit } from '@angular/core';
import { LikeModel } from '../../../../core/models/user-content/like.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LoginModel } from '../../../../core/models/auth-models/login.model';
import { MediaUrlService } from '../../../../core/services/logic/media-urls/media-url.service';
import { LikeHttpService } from '../../../../core/services/http/user-content/like-http.service';

@Component({
  selector: 'app-likers-dialog',
  templateUrl: './likers-dialog.component.html',
  styleUrl: './likers-dialog.component.scss'
})
export class LikersDialogComponent implements OnInit {
  public likes: LikeModel[];

  constructor(public mediaUrlService: MediaUrlService,
              public likeHttpService: LikeHttpService,
              public dialogRef: MatDialogRef<LikersDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: { postId: number }) { }

  public ngOnInit(): void {
    this.likeHttpService.getLikersForPost(this.data.postId).subscribe(likers => {
      this.likes = likers;
    });
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }
}
