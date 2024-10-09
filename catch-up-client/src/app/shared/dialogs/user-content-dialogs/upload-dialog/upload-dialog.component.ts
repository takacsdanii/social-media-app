import { Component, Inject, OnInit } from '@angular/core';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserProfileHttpService } from '../../../../core/services/http/user-content/user-profile-http.service';

@Component({
  selector: 'app-upload-dialog',
  templateUrl: './upload-dialog.component.html',
  styleUrl: './upload-dialog.component.scss'
})
export class UploadDialogComponent implements OnInit {
  public selectedFile: File;

  constructor(private userProfieHttpService: UserProfileHttpService,
              private notificationService: NotificationService,
              @Inject(MAT_DIALOG_DATA) public data: { userId: string, type: 'cover' | 'profile' }) { }

  public ngOnInit(): void { }

  public onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  public onUpload(): void {
    if(this.selectedFile) {
      const uploadMethod = this.data.type === 'cover'
        ? this.userProfieHttpService.editCoverPic(this.data.userId, this.selectedFile)
        : this.userProfieHttpService.editProfilePic(this.data.userId, this.selectedFile);

      uploadMethod.subscribe(resp => {
        this.notificationService.showSuccesSnackBar(`${this.data.type} picture saved succesfully`);
      });
    }
  }

  public toggleUploadType(): void {
    this.data.type = this.data.type === 'profile' ? 'cover' : 'profile';
  }
}
