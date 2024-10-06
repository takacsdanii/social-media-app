import { Component, Inject, OnInit } from '@angular/core';
import { UserContentHttpService } from '../../../../core/services/http/user-content/user-content-http.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-content-dialog',
  templateUrl: './delete-content-dialog.component.html',
  styleUrl: './delete-content-dialog.component.scss'
})
export class DeleteContentDialogComponent implements OnInit {
  public fileName: string;

  constructor(@Inject(MAT_DIALOG_DATA)
              public data: {userId: string, type: 'cover' | 'profile', imageUrl: string},
              private userContentHttpService: UserContentHttpService,) { }

  public ngOnInit(): void {
    this.fileName = this.data.imageUrl.split('/').pop()!!;
  }

  public onDelete(): void {
    if(this.data.type === 'cover')
      this.userContentHttpService.deleteCoverPic(this.data.userId, this.fileName).subscribe();
    else
      this.userContentHttpService.deleteProfilePic(this.data.userId, this.fileName).subscribe();
  }
}
