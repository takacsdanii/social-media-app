import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserPreviewModel } from '../../../models/user-preview.model';

@Injectable({
  providedIn: 'root'
})
export class FriendsHttpService {
  private url = 'https://localhost:7175/api/friends'

  constructor(private http: HttpClient) { }

  public getFollowers(userId: string): Observable<UserPreviewModel[]> {
    const link = `${this.url}/followers?userId=${userId}`;
    return this.http.get<UserPreviewModel[]>(link);
  }

  public getFollowing(userId: string): Observable<UserPreviewModel[]> {
    const link = `${this.url}/following?userId=${userId}`;
    return this.http.get<UserPreviewModel[]>(link);
  }

  public getFriends(userId: string): Observable<UserPreviewModel[]> {
    const link = `${this.url}/friends?userId=${userId}`;
    return this.http.get<UserPreviewModel[]>(link);
  }

  public doesUserFollowTargetUser(userId: string, targetUserId: string): Observable<{result: boolean}> {
    const link = `${this.url}/following-given-user?userId=${userId}&targetUserId=${targetUserId}`;
    return this.http.get<{result: boolean}>(link);
  }

  public followUser(userId: string, targetUserId: string): Observable<void> {
    const link = `${this.url}?userId=${userId}&targetUserId=${targetUserId}`;
    return this.http.post<void>(link, null);
  }

  public unFollowUser(userId: string, targetUserId: string): Observable<void> {
    const link = `${this.url}?userId=${userId}&targetUserId=${targetUserId}`;
    return this.http.delete<void>(link);
  }
}
