import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentModel } from '../../../models/user-content/comment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentHttpService {
  private url = 'https://localhost:7175/api/comments';

  constructor(private http: HttpClient) { }

  private getToken(): string | null {
    return localStorage.getItem('token');
  }

  public addCommentToPost(userId: string, postId: number, text: string): Observable<void> {
    const link = `${this.url}/comment-post?userId=${userId}&postId=${postId}&text=${text}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.post<void>(link, { headers });
  }

  public addReplyToComment(userId: string, postId: number, parentCommentId: number, text: string): Observable<number> {
    const link = `${this.url}/comment-comment?userId=${userId}&postId=${postId}&parentCommentId=${parentCommentId}&text=${text}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.post<number>(link, { headers });
  }

  public deleteComment(id: number): Observable<void> {
    const link = `${this.url}?id=${id}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.delete<void>(link, { headers });
  }

  public getCommentsForPost(postId: number): Observable<CommentModel[]> {
    const link = `${this.url}/comments-for-post?postId=${postId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<CommentModel[]>(link, { headers });
  }

  public getRepliesForComment(postId: number, parentCommentId: number): Observable<CommentModel[]> {
    const link = `${this.url}/replies-for-comment?postId=${postId}&parentCommentId=${parentCommentId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<CommentModel[]>(link, { headers });
  }

  public getCommentById(id: number): Observable<CommentModel> {
    const link = `${this.url}?id=${id}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<CommentModel>(link, { headers });
  }
}
