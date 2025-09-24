import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Navbar } from './navbar/navbar';
import { Footer } from './footer/footer';

@NgModule({
  imports: [CommonModule, Navbar, Footer], 
  exports: [Navbar, Footer, CommonModule]
})
export class SharedModule {}
