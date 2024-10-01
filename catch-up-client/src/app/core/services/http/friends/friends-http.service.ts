import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DisplayUserModel } from '../../../models/display.user.model';

@Injectable({
  providedIn: 'root'
})
export class FriendsHttpService {
  private url = 'https://localhost:7175/api/friends'

  constructor(private http: HttpClient) { }

  public getFollowers(userId: string): Observable<DisplayUserModel[]> {
    const link = `${this.url}/followers?userId=${userId}`;
    return this.http.get<DisplayUserModel[]>(link);
  }

  public getFollowing(userId: string): Observable<DisplayUserModel[]> {
    const link = `${this.url}/following?userId=${userId}`;
    return this.http.get<DisplayUserModel[]>(link);
  }

  public getFriends(userId: string): Observable<DisplayUserModel[]> {
    const link = `${this.url}/friends?userId=${userId}`;
    return this.http.get<DisplayUserModel[]>(link);
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
