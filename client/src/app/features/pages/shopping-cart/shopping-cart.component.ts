import { Component, OnInit } from '@angular/core';
import { CartService } from "../../../shared/services/cart.service";
import { ProductService } from "../../../shared/services/product.service";
import { cartItem } from "../../../models/cartItem";
import { cart } from "../../../models/cart";
import { ShopParams } from "../../../models/shopParams";
import { Product } from "../../../models/product";
import {Pagination} from "../../../models/pagination";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {
  pagination: Pagination<cartItem []> | null = null;
  cartItems: cartItem [] = []
  products: { [key: number]: Product } = {};
  shopParams: ShopParams = {
    pageIndex: 0,
    pageSize: 10,
    sort: 'name'
  };
  isOrderFormVisible = false;
  orderForm: FormGroup;


  constructor(
    private cartService: CartService,
    private productService: ProductService,
    private fb: FormBuilder
  ) {
    this.orderForm = this.fb.group({
      shippingAddress: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit() {
    this.loadCartItems();
  }

  loadCartItems() {
    this.cartService.getCartItems(this.shopParams).subscribe({
      next: (response) => {
        this.cartItems = response.data;
        this.pagination = response;
        this.loadProductsForCartItems();


        console.log('Loaded cart items:', this.cartItems);
      },
      error: (error) => console.log(error)
    });
  }

  loadProductsForCartItems() {
    this.cartItems.forEach(cartItem => {
      this.productService.getProduct(cartItem.productId).subscribe({
        next: (product) => {
          this.products[cartItem.productId] = product;
          console.log('Loaded cart products:', this.products);
        },
        error: (error) => console.log(error)
      });
    });
  }

  updateCartItem(cartItem: cartItem) {
    this.cartService.updateCartItem(cartItem.id, cartItem).subscribe((updatedItem) => {
      const index = this.cartItems.findIndex((item) => item.id === updatedItem.id);
      this.cartItems[index] = updatedItem;
    });
  }


  removeItem(item: cartItem) {
    this.cartService.softDeleteCartItem(item.id).subscribe({
      next: (updatedItem) => {
        // Находим индекс элемента в массиве
        const index = this.cartItems.findIndex(cartItem => cartItem.id === updatedItem.id);
        if (index !== -1) {
          // Удаляем элемент из отображения
          this.cartItems.splice(index, 1);
          delete this.products[item.productId];

          this.loadCartItems();
          this.getTotalPrice();
        }
      },
      error: (error) => {
        console.error('Error removing item:', error);
        // Можно добавить уведомление пользователю об ошибке
      }
    });
  }

  decrementQuantity(cartItem: cartItem): void {
    if (cartItem.quantity > 1) {
      cartItem.quantity--;
      cartItem.totalPrice = cartItem.quantity * cartItem.unitPrice;
      this.updateCartItem(cartItem);
    }
  }

  incrementQuantity(cartItem: cartItem): void {
    if (this.products[cartItem.productId]?.stock && cartItem.quantity < this.products[cartItem.productId].stock) {
      cartItem.quantity++;
      cartItem.totalPrice = cartItem.quantity * cartItem.unitPrice;
      this.updateCartItem(cartItem);
    }
  }


  getTotalItems(): number {
    return this.cartItems.reduce((total, item) => total + item.quantity, 0);
  }

  getTotalPrice(): number {
    return this.cartItems.reduce((total, item) => total + item.totalPrice, 0);
  }

  showOrderForm() {
    this.isOrderFormVisible = true;
  }

  hideOrderForm() {
    this.isOrderFormVisible = false;
    this.orderForm.reset();
  }

  submitOrder() {
    if (this.orderForm.valid) {
      const orderData = {
        ShippingAddress: this.orderForm.value.shippingAddress,
        PhoneNumber: this.orderForm.value.phoneNumber,
        Email: this.orderForm.value.email
      };

      this.cartService.createOrder(orderData).subscribe({
        next: (response) => {
          console.log('Order submitted successfully:', response);
          this.hideOrderForm();
        },
        error: (error) => {
          console.error('Error submitting order:', error);
        }
      });
    } else {
      Object.keys(this.orderForm.controls).forEach(key => {
        const control = this.orderForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
    }
  }
}
