import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ProductService } from '../../core/product.service';
import { environment } from '../../../environments/environment';




interface Product {
  id: number;
  productCode: string;
  name: string;
  category: string;
  imagePath?: string;
  price: number;
  minQuantity: number;
  discountRate: number;
}

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list.html',
  styleUrls: ['./product-list.css']
})
export class ProductList implements OnInit {
  products$!: Observable<Product[]>;

  constructor(private http: HttpClient, private router: Router, private productService: ProductService) {}

  ngOnInit(): void {
    
    this.productService.loadProducts().subscribe(); 
  }
  products: Product[] = [];
  error = '';

  

  
  loadProducts() {
    const token = localStorage.getItem('accessToken');
    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    this.http.get<Product[]>(`${environment.apiBaseUrl}/products`, { headers })
      .subscribe({
        next: res => this.products = res,
        error: err => this.error = 'Failed to load products'
      });
  }
}
