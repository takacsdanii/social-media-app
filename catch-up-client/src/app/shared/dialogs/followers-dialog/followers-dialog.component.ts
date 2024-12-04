import { Component, Inject } from '@angular/core';
import { UserPreviewModel } from '../../../core/models/user-preview.model';
import { FriendsHttpService } from '../../../core/services/http/friends/friends-http.service';
import { MediaUrlService } from '../../../core/services/logic/helpers/media-url.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-followers-dialog',
  templateUrl: './followers-dialog.component.html',
  styleUrl: './followers-dialog.component.scss'
})
export class FollowersDialogComponent {
  public users: UserPreviewModel[];

  constructor(private mediaUrlService: MediaUrlService,
              private friendsHttpService: FriendsHttpService,
              private dialogRef: MatDialogRef<FollowersDialogComponent>,
              @Inject(MAT_DIALOG_DATA) private data: { userId: string, isFollowersList: boolean }) { }

  public ngOnInit(): void {
    if(this.data.isFollowersList) {
      this.getFollowers();
    }
    else {
      this.getFollowedUsers();
    }
  }

  private getFollowers(): void {
    this.friendsHttpService.getFollowers(this.data.userId).subscribe(followers => {
      this.users = followers;
    });
  }

  private getFollowedUsers(): void {
    this.friendsHttpService.getFollowing(this.data.userId).subscribe(following => {
      this.users = following;
    });
  }

  public getProfilePicUrl(user: UserPreviewModel): string | null {
    return user ? this.mediaUrlService.getFullUrl(user.profilePicUrl) : null;
  }

  public closeDialog(): void {
    this.dialogRef.close();
  }
}
