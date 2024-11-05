import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { ViewportScroller } from '@angular/common';
import { PostHttpService } from '../../../../core/services/http/user-content/post-http.service';
import { PostModel } from '../../../../core/models/user-content/post.model';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit {
  public user: UserModel = new UserModel();
  public posts: PostModel[];

  constructor(private route: ActivatedRoute,
              private postHttpService: PostHttpService,
              private userHttpService: UserHttpService) { }

  public ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const userIdFromRoute = params.get('id');
      if (userIdFromRoute) {
        this.setUser(userIdFromRoute);
      }
    });
  }

  public setUser(userId: string): void {
    this.userHttpService.getUser(userId).subscribe(_user => {
      this.user = _user;
      this.postHttpService.getPostsOfFollowedUsers(userId).subscribe(results => {
        this.posts = results;
        console.log(this.posts);
      })
    });
  }
}