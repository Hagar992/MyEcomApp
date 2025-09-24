import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css']
})
export class Navbar {
   constructor( private router: Router) {}

  isLoggedIn = !!localStorage.getItem('accessToken');

  logout() 
  {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    window.location.href = '/login';
  }
  goToRegister() 
  {
    this.router.navigate(['/register']);
  }
  goToLogin() 
  { this.router.navigate(['/login']);

  }
   goToproductlist() 
  { this.router.navigate(['/product-list']);

  }
}
