import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import {Product, ProductPrice} from "../../models/product";
import { ProductService } from "../services/product.service";
import { ShopParams } from "../../models/shopParams";
import {Router} from "@angular/router";
import {CartService} from "../services/cart.service";
import {cart} from "../../models/cart";
import {cartItem} from "../../models/cartItem";

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit, OnChanges {
   products: Product[] = [];
  product: Product | undefined;
  cart: cart = {
    id: 0,
    userID: '',
    cartItems: []
  };

  @Input()  shopParams: ShopParams = {};


  constructor(private productService: ProductService,
              private router: Router,
              private cartService: CartService) {}

  ngOnInit() {
    this.loadProducts();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['shopParams']) {
      this.loadProducts();
    }
  }

  loadProducts() {
    this.productService.getProducts(this.shopParams).subscribe({
      next: response => {
        this.products = response.data;
        console.log('Loaded products:', this.products);
      },
      error: error => console.log(error)
    });
  }

  getMinPrice(prices: ProductPrice[]): number {
    return Math.min(...prices.map(p => p.price));
  }

  addToCart(product: Product) {
    const item: cartItem = {
      cartId: 0, id: 0, totalPrice: 0, unitPrice: 0,
      productId: product.id,
      quantity: 1,
      priceId: product!.productPrices![0].id,
    };

    const cart: cart = {
      id: 0, userID: '',
      cartItems: [item]
    };

    this.cartService.addToCart(cart).subscribe()
  }
}
