import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PostHttpService } from '../../../../core/services/http/user-content/post-http.service';
import { PostModel } from '../../../../core/models/user-content/post.model';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.scss'
})
export class PostsComponent implements OnInit, OnChanges {
  @Input() public userId: string;
  public posts: PostModel[];

  constructor(private postHttpService: PostHttpService) { }

  public ngOnInit(): void { }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes['userId'] && this.userId) {
      this.loadPosts();
    }
  }

  private loadPosts(): void {
    this.postHttpService.getPostsOfUser(this.userId).subscribe(results => {
      this.posts = results;
    });
  }
}
