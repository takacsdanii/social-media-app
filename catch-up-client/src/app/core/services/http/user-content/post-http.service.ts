import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VisibilityModel } from '../../../models/enums/visibility.model';
import { Observable } from 'rxjs';
import { PostModel } from '../../../models/user-content/post.model';

@Injectable({
  providedIn: 'root'
})
export class PostHttpService {
  private url = 'https://localhost:7175/api/user-content/post';

  constructor(private http: HttpClient) { }

  private getToken(): string | null {
    return localStorage.getItem('token');
  }

  public getPostsOfUser(userId: string): Observable<PostModel[]> {
    const link = `${this.url}/get-posts-of-user?userId=${userId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<PostModel[]>(link, { headers });
  }

  public getPost(postId: number): Observable<PostModel> {
    const link = `${this.url}/post-by-id?postId=${postId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<PostModel>(link, { headers });
  }

  //public uploadPost(postModel: UploadPostModel, files: File[]): Observable<PostModel> {
    public uploadPost(userId: string, description: string, visibility: VisibilityModel, files: File[]): Observable<PostModel> {
      //const link = `${this.url}/post`;
      const link = `${this.url}/post?userId=${userId}&description=${description}&visibility=${visibility}`;
      const headers = new HttpHeaders({
        Authorization: `Bearer ${this.getToken()}`
      });
  
      const formData = new FormData();
      for(let file of files) {
        formData.append('file', file); 
      }
      return this.http.post<PostModel>(link, formData, { headers });
    }

    public editDescription(postId: number, description: string | null): Observable<void> {
      var link = `${this.url}/description?postId=${postId}`;
      if(description != null) link = `${link}&description=${description}`;
      
      const headers = new HttpHeaders({
        Authorization: `Bearer ${this.getToken()}`
      });

      return this.http.put<void>(link, null, { headers });
    }

    public editVisibility(postId: number, visibility: VisibilityModel): Observable<void> {
      const link = `${this.url}/visibility?postId=${postId}&visibility=${visibility}`;
      const headers = new HttpHeaders({
        Authorization: `Bearer ${this.getToken()}`
      });

      return this.http.put<void>(link, null, { headers });
    }
}
