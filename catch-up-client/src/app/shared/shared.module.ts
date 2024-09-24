import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonLayoutsModule } from '../modules/common-layouts/common-layouts.module';
import { UserComponent } from './user/user.component';
import { UserDetailsComponent } from './user/user-details/user-details.component';
import { RouterModule } from '@angular/router';
import { DeleteDialogComponent } from './dialogs/delete-dialog/delete-dialog.component';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';



@NgModule({
  declarations: [
    UserComponent,
    UserDetailsComponent,
    DeleteDialogComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    CommonLayoutsModule,
    MatIconModule,
    FormsModule,
    MatTooltipModule
  ]
})
export class SharedModule { }
