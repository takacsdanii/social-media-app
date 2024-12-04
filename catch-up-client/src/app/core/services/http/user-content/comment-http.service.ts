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

  public addCommentToPost(userId: string, postId: number, text: string): Observable<void> {
    const link = `${this.url}/comment-post?userId=${userId}&postId=${postId}&text=${text}`;
    return this.http.post<void>(link, null);
  }

  public addReplyToComment(userId: string, postId: number, parentCommentId: number, text: string): Observable<number> {
    const link = `${this.url}/comment-comment?userId=${userId}&postId=${postId}&parentCommentId=${parentCommentId}&text=${text}`;
    return this.http.post<number>(link, null);
  }

  public deleteComment(id: number): Observable<void> {
    const link = `${this.url}?id=${id}`;
    return this.http.delete<void>(link);
  }

  public getCommentsForPost(postId: number): Observable<CommentModel[]> {
    const link = `${this.url}/comments-for-post?postId=${postId}`;
    return this.http.get<CommentModel[]>(link);
  }

  public getRepliesForComment(postId: number, parentCommentId: number): Observable<CommentModel[]> {
    const link = `${this.url}/replies-for-comment?postId=${postId}&parentCommentId=${parentCommentId}`;
    return this.http.get<CommentModel[]>(link);
  }

  public getCommentById(id: number): Observable<CommentModel> {
    const link = `${this.url}?id=${id}`;
    return this.http.get<CommentModel>(link);
  }
}
