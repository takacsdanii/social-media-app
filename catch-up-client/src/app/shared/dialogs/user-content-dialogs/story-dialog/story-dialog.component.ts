import { Component, EventEmitter, Inject, OnDestroy, OnInit, Output } from '@angular/core';
import { StoryHttpService } from '../../../../core/services/http/user-content/story-http.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { StoryModel } from '../../../../core/models/user-content/story.model';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';
import { StoryViewersDialogComponent } from '../story-viewers-dialog/story-viewers-dialog.component';
import { DeleteContentDialogComponent } from '../delete-content-dialog/delete-content-dialog.component';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';

@Component({
  selector: 'app-story-dialog',
  templateUrl: './story-dialog.component.html',
  styleUrl: './story-dialog.component.scss'
})
export class StoryDialogComponent implements OnInit, OnDestroy {
  public stories: StoryModel[];
  public currentIndex: number = 0;
  private intervalId: any;
  private myUserId: string;
  public userName: string;

  constructor(private storyHttpService: StoryHttpService,
              private userHttpService: UserHttpService,
              private authService: AuthService,
              private mediaUrlService: MediaUrlService,
              private viewersDialog: MatDialog,
              private deleteDialog: MatDialog,
              private storyDialogRef: MatDialogRef<StoryDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: { userId: string }) { }

  public ngOnInit(): void {
    this.myUserId = this.authService.getUserId()!;
    this.getUsername();
    this.getStoriesOfUser();
  }

  private getUsername(): void {
    this.userHttpService.getUser(this.data.userId).subscribe(user => {
      this.userName = user.userName;
    });
  }

  public addViewerToStory(): void {
    this.storyHttpService.addViewerToStory(this.myUserId, this.stories[this.currentIndex].id)
      .subscribe();
  }

  public get isMyStory(): boolean {
    return this.stories && this.stories[this.currentIndex].userId == this.myUserId;
  }

  public get hasViewers(): boolean {
    return this.stories && this.stories[this.currentIndex].viewCount > 0;
  }

  public ngOnDestroy(): void {
      this.stopStoryInterval();
  }

  public getStoriesOfUser(): void {
    this.storyHttpService.getStoriesOfUser(this.data.userId).subscribe(resp => {
      this.stories = resp;
      this.startStoryInterval();
      this.addViewerToStory();
    });
  }

  public showNextStory(): void {
    if(this.currentIndex < this.stories.length - 1) {
      this.currentIndex++;
      this.addViewerToStory();
    }
    else {
      this.currentIndex = 0;
    }
    this.restartStoryInterval();
  }

  public getMediaUrl(story: StoryModel): string | null {
    return this.mediaUrlService.getFullUrl(story.mediaUrl);
  }

  private startStoryInterval(): void {
    this.intervalId = setInterval(() => {
      this.showNextStory();
    }, 5000);
  }

  private stopStoryInterval(): void {
    if(this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }

  private restartStoryInterval(): void {
    this.stopStoryInterval();
    this.startStoryInterval();
  }

  public openViewersDialog(storyId: number): void {
    this.stopStoryInterval();
    const dialogRef = this.viewersDialog.open(StoryViewersDialogComponent, {
      width: '150px',
      height: '300px',
      data: { storyId }
    });

    dialogRef.afterClosed().subscribe(resp => {
      this.startStoryInterval();
    });
  }

  public openDeleteStoryDialog(storyId: number): void {
    this.stopStoryInterval();
    const dialogRef = this.deleteDialog.open(DeleteContentDialogComponent, {
      height: '250px',
      width: '450px',
      data: { storyId }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.storyDialogRef.close();
    })
  }
}
