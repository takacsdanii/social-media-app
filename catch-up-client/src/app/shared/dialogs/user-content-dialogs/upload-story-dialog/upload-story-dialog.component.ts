import { Component, Inject, OnInit } from '@angular/core';
import { UserProfileHttpService } from '../../../../core/services/http/user-content/user-profile-http.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationService } from '../../../../core/services/logic/notifications/notification.service';
import { StoryHttpService } from '../../../../core/services/http/user-content/story-http.service';
import { UploadStoryModel } from '../../../../core/models/user-content/upload-story.model';
import { VisibilityModel } from '../../../../core/models/enums/visibility.model';

@Component({
  selector: 'app-upload-story-dialog',
  templateUrl: './upload-story-dialog.component.html',
  styleUrl: './upload-story-dialog.component.scss'
})
export class UploadStoryDialogComponent implements OnInit{
  public selectedFile: File;
  public visibility: VisibilityModel = 0;

  constructor(private storyHttpService: StoryHttpService,
              private notificationService: NotificationService,
              @Inject(MAT_DIALOG_DATA) public data: { userId: string }) { }

  public ngOnInit(): void {
      
  }

  public onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  public onUpload(): void {
    const uploadModel: UploadStoryModel = {
      userId: this.data.userId,
      visibility: this.visibility,
      file: this.selectedFile,
    }

    this.storyHttpService.uploadStory(uploadModel).subscribe(resp => {
      this.notificationService.showSuccesSnackBar('Story is uploaded successfully!');
    });
  }
}
