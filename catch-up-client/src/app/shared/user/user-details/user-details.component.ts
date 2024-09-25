import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserModel } from '../../../core/models/user.model';
import { UserHttpService } from '../../../core/services/http/user/user-http.service';
import { NotificationService } from '../../../core/services/logic/notifications/notification.service';
import { GenderModel } from '../../../core/models/gender.model';
import { AuthService } from '../../../core/services/logic/auth/auth.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../dialogs/delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.scss'
})
export class UserDetailsComponent implements OnInit {
  public user: UserModel = new UserModel();
  public loggedInUserId: string;

  public editingField: string | null = null;
  public errorMessages: string[] = [];

  constructor(private userHttpService: UserHttpService,
              private authService: AuthService,
              private route: ActivatedRoute,
              private notificationService: NotificationService,
              private deleteDialog: MatDialog) { }
  
  public ngOnInit(): void {
    this.loggedInUserId = this.authService.getUserId()!!;
  
    this.route.paramMap.subscribe((params) => {
      const userIdFromRoute = params.get('id');
      if (userIdFromRoute) {
        this.setUser(userIdFromRoute);
      }
    });
  }

  public setUser(userId: string): void {
    this.userHttpService.getUser(userId).subscribe(
      (_user) => {
        this.user = _user;
      },
      (err) => {
        this.notificationService.showErrorSnackBar("There is no user like this");
      }
    );
  }

  public getGenderName(gender: number): string {
    return GenderModel[gender];
  }

  public startEditing(field: string): void {
    this.editingField = field;
  }

  public saveEdit(): void {
    if(this.editingField == "birthDate") {
      this.onBirthDateChange();
    }
    this.userHttpService.updateUser(this.user).subscribe(
      (_user) => {
        this.user = _user;
        this.notificationService.showSuccesSnackBar("Changes saved");
      },
      (err) => {
        this.errorMessages = [];
        if (err.error && err.error.errors)
          this.errorMessages = err.error.errors.join('; ');
        else
          this.errorMessages = ["Something went wrong!"];

        this.notificationService.showErrorSnackBar(`Operation failed: ${this.errorMessages}`);
        this.discardEditing();
      }
    );
    this.editingField = null;
  }

  public discardEditing(): void {
    this.editingField = null;
    this.setUser(this.user.id);
  }

  public onBirthDateChange(): void {
    var newDateString = (<HTMLInputElement>document.getElementById("editBirthDay")).value;
    this.user.birthDate = new Date(newDateString);
  }

  public saveGenderEdit(): void {
    this.userHttpService.updateGender(this.user.id, this.user.gender).subscribe(
      (_) => {
        this.notificationService.showSuccesSnackBar("Changes saved");
      },
      (err) => {
        this.notificationService.showErrorSnackBar("Operation failed.");
      }
    );
    this.editingField = null;
  }

  public openDeleteDialog(userId: string, userName: string): void {
    this.deleteDialog.open(DeleteDialogComponent, {
      height: '300px',
      width: '400px',
      data: {userId, userName}
    });
  }

  public isCurrentUser(): boolean {
    return this.loggedInUserId === this.user.id;
  }
}
