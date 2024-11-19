import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {cartItem} from "../../models/cartItem";
import {cart} from "../../models/cart";
import {ShopParams} from "../../models/shopParams";
import {Pagination} from "../../models/pagination";


@Injectable({
  providedIn: 'root'
})
export class CartService {
  http = inject(HttpClient);
  baseApiUrl = "http://localhost:5097";


  getCartItems(shopParams: ShopParams) {
    let params = new HttpParams();
    if (shopParams.pageIndex) params = params.append('PageIndex', shopParams.pageIndex.toString());
    if (shopParams.pageSize) params = params.append('PageSize', shopParams.pageSize.toString());
    if (shopParams.categoryId) params = params.append('CategoryID', shopParams.categoryId.toString());
    if (shopParams.sort) params = params.append('sort', shopParams.sort);
    if (shopParams.search) params = params.append('search', shopParams.search.toLowerCase());
    return this.http.get<Pagination<cartItem[]>>(this.baseApiUrl + '/cart', { params });
  }

  addToCart(cart: cart) {
    return this.http.post<cart>(`${this.baseApiUrl + '/cart'}`, cart);
  }

  updateCartItem(cartItemId: number, cartItem: cartItem) {
    return this.http.put<cartItem>(`${this.baseApiUrl + '/cart'}/${cartItemId}`, cartItem);
  }

  softDeleteCartItem(cartItemId: number) {
    return this.http.delete<cartItem>(`${this.baseApiUrl + '/cart'}/${cartItemId}`, {});
  }

  updateAllCartItems(cartItems: cartItem[]) {
    return this.http.put<cartItem[]>(`${this.baseApiUrl + '/cart'}/update-cart-items`, cartItems); // Указываем тип возвращаемого значения как массив
  }

  createOrder(orderData: any) {
    return this.http.post(`${this.baseApiUrl + '/order'}`, orderData);
  }





}

