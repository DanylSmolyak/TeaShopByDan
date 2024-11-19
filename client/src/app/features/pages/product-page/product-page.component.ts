import {Component, OnInit} from '@angular/core';
import {Product, ProductPrice} from "../../../models/product";
import {ProductService} from "../../../shared/services/product.service";
import {ProductPageService} from "../../../shared/services/product-page.service";
import {ActivatedRoute} from "@angular/router";
import {CartService} from "../../../shared/services/cart.service";
import {cart} from "../../../models/cart";
import {cartItem} from "../../../models/cartItem";


@Component({
  selector: 'app-product-page',
  templateUrl: './product-page.component.html',
  styleUrls: ['./product-page.component.css']
})
export class ProductPageComponent implements OnInit {
  product: Product | null = null;
  currentPhotoIndex = 0;
  selectedPrice: ProductPrice | null = null;
  quantity = 1;
  defaultRating = 3;
  cart: cart = {
    id: 0,
    userID: '',
    cartItems: []
  };

  constructor(
    private route: ActivatedRoute,
    private productService: ProductPageService,
    private cartService: CartService
  ) {
  }

  ngOnInit(): void {
    const productId = this.route.snapshot.params['id'];
    this.productService.getProduct(productId).subscribe({
      next: (product) => {
        this.product = product;
        this.selectedPrice = product.productPrices![0];
        console.log('Loaded product:', this.product);
      },
      error: (error) => console.error('Error loading product:', error)
    });
  }

  nextPhoto() {
    if (this.product?.photos) {
      this.currentPhotoIndex = (this.currentPhotoIndex + 1) % this.product.photos.length;
    }
  }

  previousPhoto() {
    if (this.product?.photos) {
      this.currentPhotoIndex = (this.currentPhotoIndex - 1 + this.product.photos.length) % this.product.photos.length;
    }
  }

  selectProductPrice(price: ProductPrice) {
    this.selectedPrice = price;
    console.log('Selected price updated:', this.selectedPrice);
  }

  decrementQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  incrementQuantity() {
    if (this.quantity < this.product!.stock) {
      this.quantity++;
    }
  }

  addToCart() {
    if (!this.product) return;

    const item: cartItem = {
      cartId: 0, id: 0, totalPrice: 0, unitPrice: 0,
      productId: this.product.id,
      quantity: this.quantity,
      priceId: this.selectedPrice!.id
    };

    const cart: cart = {
      id: 0, userID: '',
      cartItems: [item]
    };

    this.cartService.addToCart(cart).subscribe()
  }
}
