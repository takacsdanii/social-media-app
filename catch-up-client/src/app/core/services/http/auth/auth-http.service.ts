import { Injectable } from '@angular/core';
import { LoginModel } from '../../../models/auth-models/login.model';
import { Observable } from 'rxjs';
import { RegisterModel } from '../../../models/auth-models/register.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import jwtDecode from 'jwt-decode';
import { ChangePasswordModel } from '../../../models/auth-models/change-password.model';
import { ResetPasswordModel } from '../../../models/auth-models/reset-password.model';

@Injectable({
  providedIn: 'root'
})
export class AuthHttpService {
  private url = 'https://localhost:7175/api/auth'

  constructor(private http: HttpClient) { }

  private getToken(): string | null {
    return localStorage.getItem('token');
  }

  public register(registerModel: RegisterModel): Observable<void> {
    const link = `${this.url}/register`;
    return this.http.post<void>(link, registerModel);
  }

  public login(loginModel: LoginModel): Observable<{ token: string }> {
    const link =  `${this.url}/login`;
    return this.http.post<{token: string}>(link, loginModel);
  }

  public requestNewPassword(email: string): Observable<void> {
    const link = `${this.url}/request-new-password?email=${email}`;
    return this.http.post<void>(link, email);
  }

  public changePassword(changePassworModel: ChangePasswordModel): Observable<void> {
    const link = `${this.url}/change-password`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    return this.http.post<void>(link, changePassworModel, { headers });
  }

  public resetPassword(resetPasswordModel: ResetPasswordModel): Observable<{password: string}> {
    const link = `${this.url}/reset-password`;
    return this.http.post<{password: string}>(link, resetPasswordModel);//, { headers: { 'Content-Type': 'application/json' }});
  }

}
