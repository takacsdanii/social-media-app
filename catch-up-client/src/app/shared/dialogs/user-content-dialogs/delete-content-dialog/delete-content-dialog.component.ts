import { Component, Inject, OnInit } from '@angular/core';
import { UserProfileHttpService } from '../../../../core/services/http/user-content/user-profile-http.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-content-dialog',
  templateUrl: './delete-content-dialog.component.html',
  styleUrl: './delete-content-dialog.component.scss'
})
export class DeleteContentDialogComponent implements OnInit {
  public fileName: string;

  constructor(@Inject(MAT_DIALOG_DATA) public data: {userId: string, type: 'cover' | 'profile', imageUrl: string},
              private userProfileHttpService: UserProfileHttpService) { }

  public ngOnInit(): void {
    this.fileName = this.data.imageUrl.split('/').pop()!!;
  }

  public onDelete(): void {
    if(this.data.type === 'cover')
      this.userProfileHttpService.deleteCoverPic(this.data.userId, this.fileName).subscribe();
    else
      this.userProfileHttpService.deleteProfilePic(this.data.userId, this.fileName).subscribe();
  }
}
