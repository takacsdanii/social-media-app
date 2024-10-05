import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserHttpService } from '../../../../core/services/http/user/user-http.service';
import { UserModel } from '../../../../core/models/user.model';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit {
  public user: UserModel = new UserModel();

  constructor(private route: ActivatedRoute,
              private userHttpService: UserHttpService) { }

  public ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      const userIdFromRoute = params.get('id');
      if (userIdFromRoute) {
        this.setUser(userIdFromRoute);
      }
    });
  }

  public setUser(userId: string): void {
    this.userHttpService.getUser(userId).subscribe(_user => {
      this.user = _user;
    });
  }
}