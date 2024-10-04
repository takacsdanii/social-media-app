import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UserModel } from '../../../../core/models/user.model';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { FriendsHttpService } from '../../../../core/services/http/friends/friends-http.service';
import { DisplayUserModel } from '../../../../core/models/display.user.model';
import { map, switchMap } from 'rxjs';
import { UserContentService } from '../../../../core/services/logic/user-conent/user-content.service';

@Component({
  selector: 'app-right-side-bar',
  templateUrl: './right-side-bar.component.html',
  styleUrl: './right-side-bar.component.scss'
})
export class RightSideBarComponent implements OnInit {
  public users: UserModel[] = [];
  public loggedInUserId: string;

  public followers: DisplayUserModel[] = [];
  public following: DisplayUserModel[] = [];
  public friends: DisplayUserModel[] = [];

  public displayedOneWayFollowers: DisplayUserModel[] = [];

  @Output() followPressed: EventEmitter<void> = new EventEmitter<void>();

  constructor(private userHttpService: UserHttpService,
              private authService: AuthService,
              private friendsHttpService: FriendsHttpService,
              private userContentService: UserContentService) { }

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

  // TODO: test this
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

  public setProfilePic(user: DisplayUserModel): string {
    return this.userContentService.setProfilePic2(user);
  }
}
