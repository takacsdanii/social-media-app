import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { StoriesComponent } from './user-content/stories/stories.component';
import { PostsComponent } from './user-content/posts/posts.component';
import { NavigationHeaderComponent } from './navigation-bars/navigation-header/navigation-header.component';
import { LeftSideBarComponent } from './navigation-bars/left-side-bar/left-side-bar.component';
import { RightSideBarComponent } from './navigation-bars/right-side-bar/right-side-bar.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { UserPageComponent } from './pages/user-page/user-page.component';
import { PostComponent } from './user-content/post/post.component';
import { CommentsComponent } from './user-content/comments/comments.component';



@NgModule({
  declarations: [
    NavigationHeaderComponent,
    LeftSideBarComponent,
    RightSideBarComponent,
    StoriesComponent,
    PostsComponent,
    HomePageComponent,
    UserPageComponent,
    PostComponent,
    CommentsComponent
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
