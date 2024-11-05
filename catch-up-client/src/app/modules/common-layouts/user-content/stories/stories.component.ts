import { Component, Input, OnInit } from '@angular/core';
import { StoryHttpService } from '../../../../core/services/http/user-content/story-http.service';
import { StoryModel } from '../../../../core/models/user-content/story.model';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';
import { UserModel } from '../../../../core/models/user.model';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { UploadDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/upload-dialog/upload-dialog.component';
import { UploadStoryDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/upload-story-dialog/upload-story-dialog.component';
import { StoryDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/story-dialog/story-dialog.component';
import { MediaTypeModel } from '../../../../core/models/enums/media-type.model';

@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrl: './stories.component.scss'
})
export class StoriesComponent implements OnInit {
  public user?: UserModel;
  public myUserId: string;
  public stories: StoryModel[];
  public isStoryUploaded: boolean;
  public myFirstStory?: StoryModel;
  
  constructor(private storyHttpService: StoryHttpService,
              private userHttpService: UserHttpService,
              private authService: AuthService,
              private mediaUrlService: MediaUrlService,
              private uploadDiaog: MatDialog,
              private displayContentDialog: MatDialog) { }

  public ngOnInit(): void {
    this.myUserId = this.authService.getUserId()!;
    this.getUser();
    this.loadStories();
    this.hasUserUploadedStory();
  }

  public getUser(): void {
    this.userHttpService.getUser(this.myUserId).subscribe(resp => {
      this.user = resp;
    });
  }

  public loadStories(): void {
    this.storyHttpService.getFirstStoriesOfFollowedUsers(this.myUserId).subscribe(results => {
      this.stories = results;
    });
  }

  private hasUserUploadedStory(): void {
    this.storyHttpService.hasUserUploadedStory(this.myUserId).subscribe(resp => {
      this.isStoryUploaded = resp.result;
      this.storyHttpService.getStoriesOfUser(this.myUserId).subscribe(stories => {
        this.myFirstStory = stories[0];
      });
    });
  }

  public get defaultStoryUrl(): string | null {
    if(this.isStoryUploaded && this.myFirstStory) {
      return this.mediaUrlService.getFullUrl(this.myFirstStory?.mediaContent.mediaUrl);
    }
    else {
      return this.mediaUrlService.getFullUrl(this.user?.profilePicUrl);
    }
  }

  public get isVideo(): boolean {
    return this.myFirstStory?.mediaContent.type == MediaTypeModel.Video;
  }

  public openUploadStoryDialog(userId: string): void {
    this.uploadDiaog.open(UploadStoryDialogComponent, {
      height: '250px',
      width: '450px',
      data: { userId }
    });
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
}
