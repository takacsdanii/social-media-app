import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UploadPostModel } from '../../../models/user-content/upload-post.model';
import { PostModel } from '../../../models/user-content/post.model';
import { VisibilityModel } from '../../../models/enums/visibility.model';

@Injectable({
  providedIn: 'root'
})
export class UserProfileHttpService {
  private url = 'https://localhost:7175/api/user-content/user-profile';

  constructor(private http: HttpClient) { }

  public editProfilePic(userId: string, file: File): Observable<void> {
    const link = `${this.url}/profile-picture?userId=${userId}`;

    const formData = new FormData();
    formData.append('file', file); 
    
    return this.http.put<void>(link, formData);
  }

  public editCoverPic(userId: string, file: File): Observable<void> {
    const link = `${this.url}/cover-picture?userId=${userId}`;

    const formData = new FormData();
    formData.append('file', file); 

    return this.http.put<void>(link, formData);
  }

  public deleteProfilePic(userId: string, fileName: string): Observable<void> {
    const link = `${this.url}/profile-picture?userId=${userId}&fileName=${fileName}`;
    return this.http.delete<void>(link);
  }

  public deleteCoverPic(userId: string, fileName: string): Observable<void> {
    const link = `${this.url}/cover-picture?userId=${userId}&fileName=${fileName}`;
    return this.http.delete<void>(link);
  }

  public editBio(userId: string, bio: string | null): Observable<void> {
    const link = `${this.url}/bio`;
    const body = { 'userId': userId, 'bio': bio };
    return this.http.put<void>(link, body);
  }

}
