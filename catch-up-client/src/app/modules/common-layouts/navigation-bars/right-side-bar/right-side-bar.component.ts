import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UserModel } from '../../../../core/models/user.model';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { FriendsHttpService } from '../../../../core/services/http/friends/friends-http.service';
import { UserPreviewModel } from '../../../../core/models/user-preview.model';
import { map, switchMap } from 'rxjs';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';

@Component({
  selector: 'app-right-side-bar',
  templateUrl: './right-side-bar.component.html',
  styleUrl: './right-side-bar.component.scss'
})
export class RightSideBarComponent implements OnInit {
  public users: UserModel[] = [];
  public loggedInUserId: string;

  public followers: UserPreviewModel[] = [];
  public following: UserPreviewModel[] = [];
  public friends: UserPreviewModel[] = [];

  public displayedOneWayFollowers: UserPreviewModel[] = [];

  @Output() followPressed: EventEmitter<void> = new EventEmitter<void>();

  constructor(private userHttpService: UserHttpService,
              private authService: AuthService,
              private friendsHttpService: FriendsHttpService,
              private mediaUrlService: MediaUrlService) { }

  public ngOnInit(): void {
    this.loggedInUserId = this.authService.getUserId()!!;
    this.getFollowersAndFollowing();
    this.getFriends();
    this.refreshUsers();
  }

  private refreshUsers(): void {
    this.getUsers();
  }

  // TODO: valszeg törölhető, ha megvan minden
  private getUsers(): void {
    this.userHttpService.listUsers().subscribe(users => {
      this.users = users.filter(user => user.id != this.loggedInUserId);
    });
  }

  private getFollowersAndFollowing(): void {
    const dismissedUsers = this.getDismissedUsersFromLocalStorage();
    this.friendsHttpService.getFollowers(this.loggedInUserId).pipe(
      switchMap(followers => {
        this.followers = followers;
        return this.friendsHttpService.getFollowing(this.loggedInUserId);
      }),
      map(following => {
        this.following = following;
        this.getOneWayFollowers(dismissedUsers);
      })
    ).subscribe();
  }

  private getFriends(): void {
    this.friendsHttpService.getFriends(this.loggedInUserId).subscribe(friends => {
      this.friends = friends;
    });
  }

  private getOneWayFollowers(dismissedUsers: string[]): void {
    this.displayedOneWayFollowers = this.followers.filter(follower =>
      !this.following.some(followed => followed.id === follower.id) &&
      !dismissedUsers.includes(follower.id)
    );
  }

  private getDisplayedOneWayFollowers(userId: string): void {
    this.displayedOneWayFollowers = this.displayedOneWayFollowers.filter(
      follower => follower.id !== userId
    );
  }

  public onDismissPressed(userId: string): void {
    this.getDisplayedOneWayFollowers(userId);
    this.saveDismissedUsersToLocalStorage(userId);
  }

  private saveDismissedUsersToLocalStorage(userId: string): void{
    var dismissedUsers = JSON.parse(localStorage.getItem('dismissedUsers') || '[]');
    dismissedUsers.push(userId);
    localStorage.setItem('dismissedUsers', JSON.stringify(dismissedUsers));
  }

  private getDismissedUsersFromLocalStorage(): string[] {
    return JSON.parse(localStorage.getItem('dismissedUsers') || '[]');
  }

  public onFollowPressed(targetUserId: string): void {
    this.friendsHttpService.followUser(this.loggedInUserId, targetUserId).subscribe(_ => {
      this.followPressed.emit();
      this.getDisplayedOneWayFollowers(targetUserId);
      this.getFriends();
    });
  }

  public displayedUsers: number = 3;
  public loadMore(): void {
    this.displayedUsers += 3; 
  }

  public getProfilePicUrl(user: UserPreviewModel): string | null {
    return this.mediaUrlService.getFullUrl(user.profilePicUrl);
  }

  public getUserProfilePicUrl(user: UserModel): string | null {
    return this.mediaUrlService.getFullUrl(user.profilePicUrl);
  }
}
