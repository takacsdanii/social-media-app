import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MediaUrlService } from '../../../../core/services/logic/helpers/media-url.service';

@Component({
  selector: 'app-display-content-dialog',
  templateUrl: './display-content-dialog.component.html',
  styleUrl: './display-content-dialog.component.scss'
})
export class DisplayContentDialogComponent implements OnInit {

  constructor(private mediaUrlService: MediaUrlService,
              @Inject(MAT_DIALOG_DATA) public data: { imageUrl: string }) { }

  public ngOnInit(): void { }

  public get imageUrl(): string | null {
    return this.mediaUrlService.getFullUrl(this.data.imageUrl);
  }
}
