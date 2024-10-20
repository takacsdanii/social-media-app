import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LikeHttpService {
  private url = 'https://localhost:7175/api/likes';

  constructor(private http: HttpClient) { }

  private getToken(): string | null {
    return localStorage.getItem('token');
  }

  public likePost(userId: string, postId: number): Observable<number> {
    const link = `${this.url}/like-post?userId=${userId}&postId=${postId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.post<number>(link, { headers });
  }

  public likeComment(userId: string, postId: number, commentId: number): Observable<number> {
    const link = `${this.url}/like-comment?userId=${userId}&postId=${postId}&commentId=${commentId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.post<number>(link, { headers });
  }

  public removeLike(id: number):Observable<void> {
    const link = `${this.url}?id=${id}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.delete<void>(link, {headers });
  }
}
