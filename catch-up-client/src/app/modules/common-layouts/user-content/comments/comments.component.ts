import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { UserModel } from '../../../../core/models/user.model';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { MediaUrlService } from '../../../../core/services/logic/media-urls/media-url.service';
import { CommentModel } from '../../../../core/models/user-content/comment.model';
import { CommentHttpService } from '../../../../core/services/http/user-content/comment-http.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.scss'
})
export class CommentsComponent implements OnInit {
  public loggedInUserId: string;
  public loggedInUser: UserModel;

  constructor(private authService: AuthService,
              private userHttpService: UserHttpService,
              private commentHttpService: CommentHttpService,
              public mediaUrlService: MediaUrlService) { }

  @Input() public postId: number;
  @Output() public commentAddedOrDeleted: EventEmitter<void> = new EventEmitter<void>();
  public comments: CommentModel[];
  public commentText: string | null;

  public ngOnInit(): void {
      this.loggedInUserId = this.authService.getUserId()!;
      this.userHttpService.getUser(this.loggedInUserId).subscribe(user => {
        this.loggedInUser = user;
      });

      this.loadComments();
  }

  private loadComments(): void {
    this.commentHttpService.getCommentsForPost(this.postId).subscribe(comms => {
      this.comments = comms;
    });
  }

  public postComment(): void {
    this.commentHttpService.addCommentToPost(this.loggedInUserId, this.postId, this.commentText!).subscribe(_ => {
      this.loadComments();
      this.commentAddedOrDeleted.emit();
      this.commentText = null;
    });
  }

  public get isButtonDisabled(): boolean {
    return this.commentText == null || this.commentText == '';
  }

  public deletedComment(): void {
    this.ngOnInit();
    this.commentAddedOrDeleted.emit();
  }
}
