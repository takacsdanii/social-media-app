import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService } from '../../../../core/services/logic/auth/auth.service';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';
import { CommentHttpService } from '../../../../core/services/http/user-content/comment-http.service';
import { CommentModel } from '../../../../core/models/user-content/comment.model';
import { DeleteContentDialogComponent } from '../../../../shared/dialogs/user-content-dialogs/delete-content-dialog/delete-content-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { LikeHttpService } from '../../../../core/services/http/user-content/like-http.service';
import { switchMap } from 'rxjs';
import { TimeFormatterService } from '../../../../core/services/logic/helpers/time-formatter.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.scss'
})
export class CommentComponent implements OnInit {
  public comment: CommentModel;
  public loggedInUserId: string;
  public isLiked: boolean;
  public likeId: number | null;
  private isReplySectionOpen: boolean = false;

  constructor(private likeHttpService: LikeHttpService,
              private authService: AuthService,
              private commentHttpService: CommentHttpService,
              private deleteDialog: MatDialog,
              private mediaUrlService: MediaUrlService,
              private timeFormatterService: TimeFormatterService) { }

  @Input() public commentId: number;
  @Output() public commentDeleted: EventEmitter<void> = new EventEmitter<void>(); 
  @Output() public replySectionOpened: EventEmitter<boolean> = new EventEmitter<boolean>();

  public ngOnInit(): void {
    this.loggedInUserId = this.authService.getUserId()!;
    this.getComment();
  }

  public get isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  public get timeAgo(): string | null {
    return this.comment ? this.timeFormatterService.getTimeAgo(this.comment.createdAt) : null;
  }

  public get profilePicUrl(): string | null {
    return this.comment ? this.mediaUrlService.getFullUrl(this.comment.profilePicUrl) : null;
  }

  public getComment(): void {
    this.commentHttpService.getCommentById(this.commentId).pipe(
      switchMap(comm => {
        this.comment = comm;
        return this.likeHttpService.getLikeIdForContent(this.loggedInUserId, this.comment.postId, this.comment.id);
      })
    ).subscribe(result => {
      this.likeId = result ?? null;
      this.isLiked = result != 0;
    });
  }

  public isMyComment(): boolean {
    return this.comment ? this.comment.userId == this.loggedInUserId : false;
  }

  public openDeleteDialog(): void {
    const dialogRef = this.deleteDialog.open(DeleteContentDialogComponent, {
      height: '250px',
      width: '450px',
      data: { commentId: this.comment.id }
    });

    dialogRef.afterClosed().subscribe(_ => {
      this.commentDeleted.emit();
    });
  }

  public likeComment(): void {
    if(!this.isLiked) {
      this.likeHttpService.likeComment(this.loggedInUserId, this.comment.postId, this.comment.id).subscribe(id => {
        this.likeId = id;
        this.isLiked = false;
        this.ngOnInit();
      });
    }
    else {
      this.likeHttpService.removeLike(this.likeId!).subscribe(_ => {
        this.likeId = null;
        this.isLiked = true;
        this.ngOnInit();
      });
    }
  }

  public toggleReplySection(): void {
    this.isReplySectionOpen = !this.isReplySectionOpen;
    this.replySectionOpened.emit(this.isReplySectionOpen);
  }
}
