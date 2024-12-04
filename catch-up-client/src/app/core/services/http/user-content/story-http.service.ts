import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StoryModel } from '../../../models/user-content/story.model';
import { VisibilityModel } from '../../../models/enums/visibility.model';
import { UserPreviewModel } from '../../../models/user-preview.model';
import { UploadStoryModel } from '../../../models/user-content/upload-story.model';

@Injectable({
  providedIn: 'root'
})
export class StoryHttpService {
  private url = 'https://localhost:7175/api/user-content/story';

  constructor(private http: HttpClient) { }

  public getStoriesOfUser(userId: string): Observable<StoryModel[]> {
    const link = `${this.url}/get-stories-of-user?userId=${userId}`;
    return this.http.get<StoryModel[]>(link);
  }

  public getVisibleStoriesOfUser(storyOwnerId: string, loggedInUserId: string): Observable<StoryModel[]> {
    const link = `${this.url}/get-visible-stories-of-user?storyOwnerId=${storyOwnerId}&loggedInUserId=${loggedInUserId}`;
    return this.http.get<StoryModel[]>(link);
  }

  public getStory(storyId: number): Observable<StoryModel> {
    const link = `${this.url}/story-by-id?storyId=${storyId}`;
    return this.http.get<StoryModel>(link);
  }

  public uploadStory(uploadStoryModel: UploadStoryModel): Observable<StoryModel> {
    const formData = new FormData();
    formData.append('userId', uploadStoryModel.userId);
    formData.append('visibility', uploadStoryModel.visibility.toString());
    formData.append('file', uploadStoryModel.file); 
    
    return this.http.post<StoryModel>(this.url, formData);
  }

  public delete(storyId: number): Observable<void> {
    const link = `${this.url}?storyId=${storyId}`;
    return this.http.delete<void>(link);
  }

  public getStoryViewers(storyId: number): Observable<UserPreviewModel[]> {
    const link = `${this.url}/viewers?storyId=${storyId}`;
    return this.http.get<UserPreviewModel[]>(link);
  }

  public addViewerToStory(userId: string, storyId: number): Observable<void> {
    const link = `${this.url}/add-viewer-to-story?userId=${userId}&storyId=${storyId}`;
    return this.http.post<void>(link, null);
  }

  public hasUserUploadedStory(userId: string): Observable<{ result: boolean}> {
    const link = `${this.url}/has-user-uploaded-story?userId=${userId}`;
    return this.http.get<{ result: boolean }>(link,);
  }

  public hasUserUploadedVisibleStoryForLoggedInUser(storyOwnerId: string, loggedInUserId: string): Observable<{ result: boolean}> {
    const link = `${this.url}/has-user-uploaded-story?storyOwnerId=${storyOwnerId}&loggedInUserId=${loggedInUserId}`;
    return this.http.get<{ result: boolean }>(link);
  }

  public getFirstStoriesOfFollowedUsers(userId: string): Observable<StoryModel[]> {
    const link = `${this.url}/first-stories-of-followed-users?userId=${userId}`;

    return this.http.get<StoryModel[]>(link);
  }
}
