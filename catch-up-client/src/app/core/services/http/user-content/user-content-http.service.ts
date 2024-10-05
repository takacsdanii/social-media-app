import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserContentHttpService {
  private url = 'https://localhost:7175/api/user-content'

  constructor(private http: HttpClient) { }

  private getToken(): string | null {
    return localStorage.getItem('token');
  }

  public editProfilePic(userId: string, file: File): Observable<void> {
    const link = `${this.url}/profile-picture?userId=${userId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    const formData = new FormData();
    formData.append('file', file); 
    
    return this.http.put<void>(link, formData, { headers });
  }

  public editCoverPic(userId: string, file: File): Observable<void> {
    const link = `${this.url}/cover-picture?userId=${userId}`;
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.getToken()}`
    });

    const formData = new FormData();
    formData.append('file', file); 

    return this.http.put<void>(link, formData, { headers });
  }
}
