import { Component, Inject } from '@angular/core';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';
import { UserPreviewModel } from '../../../../core/models/user-preview.model';
import { StoryHttpService } from '../../../../core/services/http/user-content/story-http.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-story-viewers-dialog',
  templateUrl: './story-viewers-dialog.component.html',
  styleUrl: './story-viewers-dialog.component.scss'
})
export class StoryViewersDialogComponent {
  public viewers: UserPreviewModel[];

  constructor(private mediaUrlService: MediaUrlService,
              private storyHttpService: StoryHttpService,
              @Inject(MAT_DIALOG_DATA) private data: { storyId: number }) { }

  public ngOnInit(): void {
    this.getViewersOfStory();
  }

  private getViewersOfStory(): void {
    this.storyHttpService.getStoryViewers(this.data.storyId).subscribe(users => {
      this.viewers = users;
    });
  }

  public getProfilePicUrl(viewer: UserPreviewModel): string | null {
    return this.mediaUrlService.getFullUrl(viewer.profilePicUrl);
  }
}
