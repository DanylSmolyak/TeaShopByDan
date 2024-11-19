import { Component, OnInit } from '@angular/core';
import { ProductService } from "../../../shared/services/product.service";
import { Pagination } from "../../../models/pagination";
import { PageEvent } from '@angular/material/paginator';
import { ShopParams } from "../../../models/shopParams";
import {Product, ProductPrice} from "../../../models/product";
import {CartService} from "../../../shared/services/cart.service";
import {cartItem} from "../../../models/cartItem";
import {cart} from "../../../models/cart";
import {ActivatedRoute} from "@angular/router";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-teas',
  templateUrl: './teas.component.html',
  styleUrls: ['./teas.component.css']
})export class TeasComponent implements OnInit {
  pagination: Pagination<Product[]> | null = null;
  products: Product[] = [];
  shopParams: ShopParams = {
    pageIndex: 0,
    pageSize: 20,
    sort: 'name',
    search: ''
  };
  product: Product | undefined;
  cart: cart = {
    id: 0,
    userID: '',
    cartItems: []
  };

  selectedFilters: string[] = [];




  constructor(private productService: ProductService,
              private cartService: CartService,
              private route: ActivatedRoute) {}

  ngOnInit() {
    // Subscribe to query params to handle search
    this.route.queryParams.subscribe(params => {
      if (params['search']) {
        this.shopParams.search = params['search'];
      } else {
        this.shopParams.search = '';
      }
      this.loadProducts();
    });
  }

  loadProducts() {
    this.productService.getProducts(this.shopParams).subscribe({
      next: response => {
        this.products = response.data;
        this.pagination = response;
        console.log('Loaded products:', this.products);
      },
      error: error => console.log(error)
    });
  }

  onPageChange(event: PageEvent) {
    this.shopParams.pageIndex = event.pageIndex + 1 ;
    this.shopParams.pageSize = event.pageSize;
    this.loadProducts();
  }

  sortSelected(sortOption: string) {
    this.shopParams.sort = sortOption;
    this.shopParams.pageIndex = 0;
    this.loadProducts();
  }

  categorySelect(categoryId: number) {
    this.shopParams.categoryId = categoryId;
    this.shopParams.pageIndex = 0;
    this.loadProducts();
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

  addFilter(filter: string) {
    if (!this.selectedFilters.includes(filter)) {
      this.selectedFilters.push(filter);
    }
  }

  removeFilter(filter: string) {
    this.selectedFilters = this.selectedFilters.filter(f => f !== filter);
  }

  clearFilters() {
    this.selectedFilters = [];
  }
}
