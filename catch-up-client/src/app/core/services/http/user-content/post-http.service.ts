import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VisibilityModel } from '../../../models/enums/visibility.model';
import { Observable } from 'rxjs';
import { PostModel } from '../../../models/user-content/post.model';
import { UploadPostModel } from '../../../models/user-content/upload-post.model';

@Injectable({
  providedIn: 'root'
})
export class PostHttpService {
  private url = 'https://localhost:7175/api/user-content/post';

  constructor(private http: HttpClient) { }

  public getPostsOfUser(userId: string): Observable<PostModel[]> {
    const link = `${this.url}/get-posts-of-user?userId=${userId}`;
    return this.http.get<PostModel[]>(link);
  }

  public getVisiblePostsOfUser(postOwnerId: string, loggedInUserId: string): Observable<PostModel[]> {
    const link = `${this.url}/get-visible-posts-of-user?postOwnerId=${postOwnerId}&loggedInUserId=${loggedInUserId}`;
    return this.http.get<PostModel[]>(link);
  }

  public getPost(postId: number): Observable<PostModel> {
    const link = `${this.url}/post-by-id?postId=${postId}`;
    return this.http.get<PostModel>(link);
  }

  public uploadPost(postModel: UploadPostModel, files: File[]): Observable<PostModel> {
    const formData = new FormData();

    formData.append('userId', postModel.userId);
    if(postModel.description !== null) {
      formData.append('description', postModel.description);
    }
    formData.append('visibility', postModel.visibility.toString());

    for(let file of files) {
      formData.append('files', file); 
    }
    
    return this.http.post<PostModel>(this.url, formData);
  }

  public editDescription(postId: number, description: string | null): Observable<void> {
    var link = `${this.url}/description?postId=${postId}`;
    if(description != null) link = `${link}&description=${description}`;

    return this.http.put<void>(link, null);
  }

  public editVisibility(postId: number, visibility: VisibilityModel): Observable<void> {
    const link = `${this.url}/visibility?postId=${postId}&visibility=${visibility}`;
    return this.http.put<void>(link, null);
  }

  public delete(postId: number): Observable<void> {
    const link = `${this.url}?postId=${postId}`;
    return this.http.delete<void>(link);
  }

  public getPostsOfFollowedUsers(userId: string): Observable<PostModel[]> {
    const link = `${this.url}/posts-of-followed-users?userId=${userId}`;
    return this.http.get<PostModel[]>(link);
  }
}
