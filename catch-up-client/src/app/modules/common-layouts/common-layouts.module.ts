import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationHeaderComponent } from './navigation-header/navigation-header.component';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { LeftSideBarComponent } from './side-bar/left-side-bar.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RightSideBarComponent } from './right-side-bar/right-side-bar.component';



@NgModule({
  declarations: [
    NavigationHeaderComponent,
    LeftSideBarComponent,
    RightSideBarComponent
  ],
  
  exports: [
    NavigationHeaderComponent,
    LeftSideBarComponent,
    RightSideBarComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatToolbarModule,
    MatButtonModule,
    MatMenuModule,
    MatTooltipModule
  ]
})
export class CommonLayoutsModule { }
