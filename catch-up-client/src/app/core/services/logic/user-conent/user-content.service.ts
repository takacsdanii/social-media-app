import { Injectable } from '@angular/core';
import { UserModel } from '../../../models/user.model';
import { DisplayUserModel } from '../../../models/display.user.model';

@Injectable({
  providedIn: 'root'
})
export class UserContentService {

  constructor() { }

  public setProfilePic(user: UserModel): string {
    var baseUrl: string = "https://localhost:7175";
    if(user.profilePicUrl != null)
      return `${baseUrl}${user.profilePicUrl}`;

    switch(user.gender) {
      case(0): return `${baseUrl}/defaults/ProfilePictures/female.png`;
      case(1): return `${baseUrl}/defaults/ProfilePictures/male.png`;
      case(2): return `${baseUrl}/defaults/ProfilePictures/other.jpg`;
    }
    return "";
  }

  public setProfilePic2(user: DisplayUserModel): string {
    var baseUrl: string = "https://localhost:7175";
    if(user.profilePicUrl != null)
      return `${baseUrl}${user.profilePicUrl}`;

    // switch(user.gender) {
    //   case(0): return `${baseUrl}/defaults/ProfilePictures/female.png`;
    //   case(1): return `${baseUrl}/defaults/ProfilePictures/male.png`;
    //   case(2): return `${baseUrl}/defaults/ProfilePictures/other.jpg`;
    // }
    return "";
  }
}
