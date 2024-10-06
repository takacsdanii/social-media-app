import { Component, Inject, OnInit } from '@angular/core';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserContentHttpService } from '../../../../core/services/http/user-content/user-content-http.service';
import { UserModel } from '../../../../core/models/user.model';

@Component({
  selector: 'app-edit-bio-dialog',
  templateUrl: './edit-bio-dialog.component.html',
  styleUrl: './edit-bio-dialog.component.scss'
})
export class EditBioDialogComponent implements OnInit {
  public bio: string | null;

  constructor(private notificationService: NotificationService,
              private userContentHttpService: UserContentHttpService,
              @Inject(MAT_DIALOG_DATA) public data: { userId: string, bio: string | null }) {}

  public ngOnInit(): void {
    this.bio = this.data.bio;
  }

  public onSave(): void {
    if(this.bio == '') {
      this.bio = null;
    }
    this.userContentHttpService.editBio(this.data.userId, this.bio).subscribe(_ => {
      this.notificationService.showSuccesSnackBar("Changes saved");
    });
  }
}
