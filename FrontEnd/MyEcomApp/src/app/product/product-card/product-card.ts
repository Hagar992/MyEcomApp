import { Component, Input } from '@angular/core';

export interface CartItem {
  title: string;
  description: string;
  price: number;
  imageUrl: string;
  quantity: number;
}

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.html',
  styleUrls: ['./product-card.css']
})
export class ProductCard {
  @Input() title: string = 'Premium Wireless Headphones';
  @Input() description: string = 'Experience crystal-clear sound with our premium wireless headphones featuring noise cancellation and 30-hour battery life.';
  @Input() price: number = 129.99;
  @Input() imageUrl: string = 'https://placehold.co/300x200/4a90e2/ffffff?text=Product+Image';
  @Input() rating: string = '★★★★☆';
  @Input() reviews: number = 42;

  addToCart() {
   
    const cart: CartItem[] = JSON.parse(localStorage.getItem('cart') || '[]');


    const existingItem = cart.find(item => item.title === this.title);
    if (existingItem) {
      existingItem.quantity += 1;
    } else {
   
      cart.push({
        title: this.title,
        description: this.description,
        price: this.price,
        imageUrl: this.imageUrl,
        quantity: 1
      });
    }

   
    localStorage.setItem('cart', JSON.stringify(cart));

    alert(`Added "${this.title}" to your cart!`);
  }
}
