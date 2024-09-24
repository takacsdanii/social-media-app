import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './shared/user/user.component';
import { UserDetailsComponent } from './shared/user/user-details/user-details.component';
import { RegisterComponent } from './modules/auth/register/register.component';
import { LoginComponent } from './modules/auth/login/login.component';
import { ForgotPasswordComponent } from './modules/auth/forgot-password/forgot-password.component';
import { ChangePasswordComponent } from './modules/auth/change-password/change-password.component';
import { ResetPasswordComponent } from './modules/auth/reset-password/reset-password.component';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: "", component: LoginComponent, canActivate: [AuthGuard] },
  { path: "users", component: UserComponent, canActivate: [AuthGuard] },
  { path: "users/:id", component: UserDetailsComponent, canActivate: [AuthGuard] },
  { path: "register", component: RegisterComponent, canActivate: [AuthGuard] },
  { path: "login", component: LoginComponent, canActivate: [AuthGuard] },
  { path: "forgot-password", component: ForgotPasswordComponent, canActivate: [AuthGuard] },
  { path: "users/:id/change-password", component: ChangePasswordComponent, canActivate: [AuthGuard]},
  { path: "reset-password/:email/:reset-token", component: ResetPasswordComponent, canActivate: [AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
