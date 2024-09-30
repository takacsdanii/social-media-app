import { Component } from '@angular/core';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  public isLiked: boolean;
  public isCommentSectionOpen: boolean = false;

  constructor() { }

  public toggleLike(): void {
    this.isLiked = ! this.isLiked;
  }

  public toggleOpenCommentSection(): void {
    this.isCommentSectionOpen = !this.isCommentSectionOpen;
  }
}
