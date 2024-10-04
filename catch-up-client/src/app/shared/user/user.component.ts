import { Component, OnInit, ViewChild } from '@angular/core';
import { UserModel } from '../../core/models/user.model';
import { UserHttpService } from '../../core/services/http/user/user-http.service';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../dialogs/delete-dialog/delete-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit{
  public dataSource = new MatTableDataSource<UserModel>();
  displayedColumns: string[] = ['userName', 'firstName', 'lastName', 'actions'];

  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  constructor(private userHttpService: UserHttpService,
              private deleteDialog: MatDialog) { }

  public ngOnInit(): void {
    this.getUsers();
  }

  public getUsers(): void {
    this.userHttpService.listUsers().subscribe(users => {
      this.dataSource.data = users;
      this.dataSource.sort = this.sort;
    });
  }

  public openDeleteDialog(userId: string, userName: string): void {
    const dialogref = this.deleteDialog.open(DeleteDialogComponent, {
      height: '300px',
      width: '400px',
      data: {userId, userName}
    });

    dialogref.afterClosed().subscribe(result => {
      this.getUsers();
    });
  }
}
