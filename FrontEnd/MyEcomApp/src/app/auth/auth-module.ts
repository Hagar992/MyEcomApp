import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Login} from './login/login';
import { Register} from './register/register';
import { authRoutes } from './auth-routing-module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(authRoutes),
    Login,       
    Register
  ],
 
})
export class AuthModule {}
