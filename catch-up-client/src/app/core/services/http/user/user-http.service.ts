import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserModel } from '../../../models/user.model';
import { GenderModel } from '../../../models/enums/gender.model';
import { SearchUserModel } from '../../../models/search-user.model';

@Injectable({
  providedIn: 'root'
})
export class UserHttpService {
  private url = 'https://localhost:7175/api/users'

  constructor(private http: HttpClient) { }

  public listUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(this.url);
  }

  public getUser(id: string): Observable<UserModel> {
    const link = `${this.url}/user-by-id?id=${id}`;
    return this.http.get<UserModel>(link);
  }

  public deleteUser(id: string): Observable<void> {
    const link = `${this.url}?id=${id}`;
    return this.http.delete<void>(link);
  }

  public getUserByEmail(email: string) : Observable<UserModel> {
    const link = `${this.url}/user-by-email?email=${email}`;
    return this.http.get<UserModel>(link);
  }

  public updateUser(user: UserModel): Observable<UserModel> {
    return this.http.put<UserModel>(this.url, user);
  }

  public updateGender(userId: string, gender: GenderModel): Observable<void> {
    const link = `${this.url}/update-gender?userId=${userId}&gender=${gender}`;
    return this.http.put<void>(link, null);
  }

  public searchUsers(searchString: string): Observable<SearchUserModel[]> {
    const link = `${this.url}/search-users?searchString=${searchString}`;
    return this.http.get<SearchUserModel[]>(link);
  }
}
