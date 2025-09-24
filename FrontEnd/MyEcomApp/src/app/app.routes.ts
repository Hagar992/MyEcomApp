import { Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { Register } from './auth/register/register';
import { ProductCard } from './product/product-card/product-card';

import { ProductList} from './product/product-list/product-list';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'ProductCard', component: ProductCard },
  { path: 'register', component: Register ,},
  { path: 'ProductList', component: ProductList },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
