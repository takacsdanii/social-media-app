import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  public isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  public logOut(): void {
    localStorage.removeItem('token');
  }

  public getUserRole(): string | null {
    const token = localStorage.getItem('token');

    if(token) {
      const decodedToken: any = jwtDecode(token);
      return decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }
    return null;
  }

  public getUserName(): string | null {
    const token = localStorage.getItem('token');

    if(token) {
      const decodedToken: any = jwtDecode(token);
      return decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    }
    return null;
  }

  public getUserId(): string | null {
    const token = localStorage.getItem('token');

    if(token) {
      const decodedToken: any = jwtDecode(token);
      return decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    }
    return null;
  }

  public isAdmin(): boolean {
    const role = this.getUserRole();
    return role == 'admin';
  }
}
