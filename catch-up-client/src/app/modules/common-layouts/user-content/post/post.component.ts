import { Component } from '@angular/core';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  public isLiked: boolean;
  public isCommentSectionOpen: boolean = false;
  public currentImgIdx: number = 0;
  public imageUrls: string[] = [
    'assets/images/logos/logo5.png',
    'assets/images/other/profile.jpg',
    'assets/images/other/dream.jpg'
  ];

  constructor() { }

  public toggleLike(): void {
    this.isLiked = ! this.isLiked;
  }

  public toggleOpenCommentSection(): void {
    this.isCommentSectionOpen = !this.isCommentSectionOpen;
  }

  public prevImage(): void {
    if(this.currentImgIdx > 0)
      this.currentImgIdx--;
  }

  public nextImage(): void {
    if(this.currentImgIdx < this.imageUrls.length - 1)
      this.currentImgIdx++;
  }
}
