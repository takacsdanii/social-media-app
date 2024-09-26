import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/logic/auth/auth.service';
import { UserModel } from '../../../core/models/user.model';

@Component({
  selector: 'app-left-side-bar',
  templateUrl: './left-side-bar.component.html',
  styleUrl: './left-side-bar.component.scss'
})
export class LeftSideBarComponent implements OnInit {
  public userName: string;

  constructor(private authService: AuthService) { }

  public ngOnInit(): void {
      this.userName = this.authService.getUserName()!!;
  }
}
