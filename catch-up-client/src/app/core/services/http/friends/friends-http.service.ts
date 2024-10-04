import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DisplayUserModel } from '../../../models/display.user.model';

@Injectable({
  providedIn: 'root'
})
export class FriendsHttpService {
  private url = 'https://localhost:7175/api/friends'

  constructor(private http: HttpClient) { }

  private getToken(): string | null {
    return localStorage.getItem('token');
  }

  public getFollowers(userId: string): Observable<DisplayUserModel[]> {
    const link = `${this.url}/followers?userId=${userId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });
    return this.http.get<DisplayUserModel[]>(link, { headers });
  }

  public getFollowing(userId: string): Observable<DisplayUserModel[]> {
    const link = `${this.url}/following?userId=${userId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });
    return this.http.get<DisplayUserModel[]>(link, { headers });
  }

  public getFriends(userId: string): Observable<DisplayUserModel[]> {
    const link = `${this.url}/friends?userId=${userId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });
    return this.http.get<DisplayUserModel[]>(link, { headers });
  }

  public doesUserFollowTargetUser(userId: string, targetUserId: string): Observable<{result: boolean}> {
    const link = `${this.url}/following-given-user?userId=${userId}&targetUserId=${targetUserId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });
    return this.http.get<{result: boolean}>(link, { headers });
  }

  public followUser(userId: string, targetUserId: string): Observable<void> {
    const link = `${this.url}?userId=${userId}&targetUserId=${targetUserId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });
    return this.http.post<void>(link, null, { headers });
  }

  public unFollowUser(userId: string, targetUserId: string): Observable<void> {
    const link = `${this.url}?userId=${userId}&targetUserId=${targetUserId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });
    return this.http.delete<void>(link, { headers });
  }
}
