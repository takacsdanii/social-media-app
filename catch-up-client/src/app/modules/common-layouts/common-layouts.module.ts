import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationHeaderComponent } from './navigation-header/navigation-header.component';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { SideBarComponent } from './side-bar/side-bar.component';
import { MatTooltipModule } from '@angular/material/tooltip';



@NgModule({
  declarations: [
    NavigationHeaderComponent,
    SideBarComponent
  ],
  
  exports: [
    NavigationHeaderComponent,
    SideBarComponent
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
