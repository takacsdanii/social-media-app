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

  private getToken(): string | null {
    return localStorage.getItem('token');
  }

  public getStoriesOfUser(userId: string): Observable<StoryModel[]> {
    const link = `${this.url}/get-stories-of-user?userId=${userId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<StoryModel[]>(link, { headers });
  }

  public getStory(storyId: number): Observable<StoryModel> {
    const link = `${this.url}/story-by-id?storyId=${storyId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<StoryModel>(link, { headers });
  }

  public uploadStory(uploadStoryModel: UploadStoryModel): Observable<StoryModel> {
    const formData = new FormData();
    formData.append('userId', uploadStoryModel.userId);
    formData.append('visibility', uploadStoryModel.visibility.toString());
    formData.append('file', uploadStoryModel.file); 
    
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });
    
    return this.http.post<StoryModel>(this.url, formData, { headers });
  }

  public editVisibility(storyId: number, visibility: VisibilityModel): Observable<void> {
    const link = `${this.url}/visibility?storyId=${storyId}&visibility=${visibility}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.put<void>(link, null, { headers });
  }

  public delete(storyId: number): Observable<void> {
    const link = `${this.url}?storyId=${storyId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.delete<void>(link, { headers });
  }

  public getStoryViewers(storyId: number): Observable<UserPreviewModel[]> {
    const link = `${this.url}/viewers?storyId=${storyId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<UserPreviewModel[]>(link, { headers });
  }

  public addViewerToStory(userId: string, storyId: number): Observable<void> {
    const link = `${this.url}/add-viewer-to-story?userId=${userId}&storyId=${storyId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.post<void>(this.url, null, { headers });
  }
}
