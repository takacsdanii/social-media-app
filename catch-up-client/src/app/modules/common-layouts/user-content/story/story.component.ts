import { Component, Input, OnInit } from '@angular/core';
import { UserModel } from '../../../../core/models/user.model';
import { StoryHttpService } from '../../../../core/services/http/user-content/story-http.service';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';
import { StoryModel } from '../../../../core/models/user-content/story.model';
import { switchMap } from 'rxjs';
import { StoryDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/story-dialog/story-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MediaTypeModel } from '../../../../core/models/enums/media-type.model';

@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrl: './story.component.scss'
})
export class StoryComponent implements OnInit {
  @Input() public storyId: number;
  public story?: StoryModel;
  public user?: UserModel;

  constructor(private storyHttpService: StoryHttpService,
              private userHttpService: UserHttpService,
              private displayContentDialog: MatDialog,
              private mediaUrlService: MediaUrlService) { }

  public ngOnInit(): void {
    this.storyHttpService.getStory(this.storyId).pipe(
      switchMap(resp => {
        this.story = resp;
        return this.userHttpService.getUser(this.story.userId);
      })
    ).subscribe(resp => {
      this.user = resp;
    });
  }

  public get profilePicUrl(): string | null {
    return this.mediaUrlService.getFullUrl(this.user?.profilePicUrl);
  }

  public get storyUrl(): string | null {
    return this.mediaUrlService.getFullUrl(this.story?.mediaContent.mediaUrl)
  }

  public get isVideo(): boolean {
    return this.story?.mediaContent.type == MediaTypeModel.Video;
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
