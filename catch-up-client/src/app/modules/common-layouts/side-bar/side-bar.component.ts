import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/logic/auth/auth.service';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.scss'
})
export class SideBarComponent implements OnInit {
  public isLoggedIn: boolean = false;
  public isAdmin: boolean = false;

  constructor(private authService: AuthService) { }

  public ngOnInit(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.isAdmin = this.authService.isAdmin();
  }
}
