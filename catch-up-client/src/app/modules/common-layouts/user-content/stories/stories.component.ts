import { Component, Input, OnInit } from '@angular/core';
import { StoryHttpService } from '../../../../core/services/http/user-content/story-http.service';
import { StoryModel } from '../../../../core/models/user-content/story.model';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';
import { UserModel } from '../../../../core/models/user.model';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';

@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrl: './stories.component.scss'
})
export class StoriesComponent implements OnInit {
  @Input() public userId: string;
  public user: UserModel;
  public stories: StoryModel[];
  
  constructor(private storyHttpService: StoryHttpService,
              private userHttpService: UserHttpService,
              private mediaUrlService: MediaUrlService) { }

  public ngOnInit(): void {
    console.log(this.userId)
    this.userId = '8f59c59e-3740-4d74-9aa6-f66f1083c13a';
    this.getUser();
    this.loadStories();
  }

  public getUser(): void {
    if(this.userId) {
      this.userHttpService.getUser(this.userId).subscribe(resp => {
        this.user = resp;
      });
    }
  }

  public loadStories(): void {
    if(this.userId) {
      this.storyHttpService.getStoriesOfUser(this.userId).subscribe(results => {
        this.stories = results;
      });
    }
  }

  public get profilePicUrl(): string | null {
    return this.mediaUrlService.getFullUrl(this.user?.profilePicUrl);
  }

}
