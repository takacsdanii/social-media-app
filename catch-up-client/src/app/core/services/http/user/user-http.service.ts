import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserModel } from '../../../models/user.model';
import jwtDecode from 'jwt-decode';
import { GenderModel } from '../../../models/enums/gender.model';
import { SearchUserModel } from '../../../models/search-user.model';

@Injectable({
  providedIn: 'root'
})
export class UserHttpService {
  private url = 'https://localhost:7175/api/users'

  constructor(private http: HttpClient) { }

  private getToken(): string | null {
    return localStorage.getItem('token');
  }

  public listUsers(): Observable<UserModel[]> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<UserModel[]>(this.url, { headers });
  }

  public getUser(id: string): Observable<UserModel> {
    const link = `${this.url}/user-by-id?id=${id}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<UserModel>(link, { headers });
  }

  public getUsersByName(name: string): Observable<UserModel[]> {
    const link = `${this.url}/users-by-name?name=${name}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<UserModel[]>(link, { headers });
  } 

  public deleteUser(id: string): Observable<void> {
    const link = `${this.url}?id=${id}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.delete<void>(link, { headers });
  }

  public getUserByEmail(email: string) : Observable<UserModel> {
    const link = `${this.url}/user-by-email?email=${email}`;
    return this.http.get<UserModel>(link);
  }

  public updateUser(user: UserModel): Observable<UserModel> {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });
    return this.http.put<UserModel>(this.url, user, { headers });
  }

  public updateGender(userId: string, gender: GenderModel): Observable<void> {
    const link = `${this.url}/update-gender?userId=${userId}&gender=${gender}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.put<void>(link, null, { headers });
  }

  public searchUsers(searchString: string): Observable<SearchUserModel[]> {
    const link = `${this.url}/search-users?searchString=${searchString}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.get<SearchUserModel[]>(link, { headers });
  }
}
