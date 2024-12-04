import { Component, Inject, OnInit } from '@angular/core';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserProfileHttpService } from '../../../../core/services/http/user-content/user-profile-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { PostHttpService } from '../../../../core/services/http/user-content/post-http.service';
import { VisibilityModel } from '../../../../core/models/enums/visibility.model';

@Component({
  selector: 'app-edit-content-dialog',
  templateUrl: './edit-content-dialog.component.html',
  styleUrl: './edit-content-dialog.component.scss'
})
export class EditContentDialogComponent implements OnInit {
  public bio?: string | null = undefined;

  public description?: string | null = undefined;
  public visibility: VisibilityModel | null = null;
  public postId: number;

  constructor(
    private notificationService: NotificationService,
    private userProfileHttpService: UserProfileHttpService,
    private postHttpService: PostHttpService,

    @Inject(MAT_DIALOG_DATA) public data: {
      userId: string,
      bio: string | null,

      postId: number,
      description: string | null,
      visibility: VisibilityModel | null
    }
  ) {}

  public ngOnInit(): void {
    this.bio = this.data.bio;
    this.description = this.data.description;
    this.visibility = this.data.visibility;
  }

  public onBioSave(): void {
    if(this.bio == '') {
      this.bio = null;
    }
    this.userProfileHttpService.editBio(this.data.userId, this.bio!).subscribe(_ => {
      this.notificationService.showSuccesSnackBar("Changes saved");
    });
  }

  public onDescriptionSave(): void {
    if(this.description == '') {
      this.description = null;
    }
    this.postHttpService.editDescription(this.data.postId, this.description!).subscribe(_ => {
      this.notificationService.showSuccesSnackBar("Changes saved");
    });
  }

  public onVisibilitySave(): void {
    this.postHttpService.editVisibility(this.data.postId, this.visibility!).subscribe(_ => {
      this.notificationService.showSuccesSnackBar("Changes saved");
    });
  }
}
