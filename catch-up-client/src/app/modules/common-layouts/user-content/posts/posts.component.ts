import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { PostHttpService } from '../../../../core/services/http/user-content/post-http.service';
import { PostModel } from '../../../../core/models/user-content/post.model';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { FriendsHttpService } from '../../../../core/services/http/friends/friends-http.service';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.scss'
})
export class PostsComponent implements OnInit, OnChanges {
  @Input() public userId: string;
  @Input() public isHomePage: boolean;
  public myUserId: string;
  public posts: PostModel[];

  constructor(private postHttpService: PostHttpService,
              private authService: AuthService) { }

  public ngOnInit(): void {
    this.myUserId = this.authService.getUserId()!;
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['userId'] && this.userId) {
      this.loadPosts();
    }
  }

  public loadPosts(): void {
    if(!this.isHomePage) {
      this.postHttpService.getVisiblePostsOfUser(this.userId, this.myUserId).subscribe(results => {
        this.posts = results;
      });
    }
    else {
      this.postHttpService.getPostsOfFollowedUsers(this.userId).subscribe(results => {
        this.posts = results;
      });
    }
  }

}
