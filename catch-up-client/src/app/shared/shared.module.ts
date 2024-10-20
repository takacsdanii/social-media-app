import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonLayoutsModule } from '../modules/common-layouts/common-layouts.module';
import { UserComponent } from './user/user.component';
import { UserDetailsComponent } from './user/user-details/user-details.component';
import { RouterModule } from '@angular/router';
import { DeleteUserDialogComponent } from './dialogs/delete-user-dialog/delete-user-dialog.component';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { UploadDialogComponent } from './dialogs/user-content-dialogs/upload-dialog/upload-dialog.component';
import { DisplayContentDialogComponent } from './dialogs/user-content-dialogs/display-content-dialog/display-content-dialog.component';
import { DeleteContentDialogComponent } from './dialogs/user-content-dialogs/delete-content-dialog/delete-content-dialog.component';
import { EditContentDialogComponent } from './dialogs/user-content-dialogs/edit-bio-dialog/edit-content-dialog.component';
import { LikersDialogComponent } from './dialogs/user-content-dialogs/likers-dialog/likers-dialog.component';



@NgModule({
  declarations: [
    UserComponent,
    UserDetailsComponent,
    DeleteUserDialogComponent,
    UploadDialogComponent,
    DisplayContentDialogComponent,
    DeleteContentDialogComponent,
    EditContentDialogComponent,
    LikersDialogComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    CommonLayoutsModule,
    MatIconModule,
    FormsModule,
    MatTooltipModule,
    MatDialogModule,
    MatSortModule,
    MatTableModule,
    MatButtonToggleModule
  ]
})
export class SharedModule { }
