import { Component, signal } from '@angular/core'; 
import { RouterModule } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { Navbar } from './shared/navbar/navbar';
import { Footer } from './shared/footer/footer';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  standalone: true,
  imports: [Navbar,RouterOutlet, Footer, RouterModule],
})
export class App {
  protected readonly title = signal('MyEcomApp');
}
