import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import {BehaviorSubject, Observable} from 'rxjs';
import { Product } from "../../models/product";
import { Pagination } from "../../models/pagination";
import { ShopParams } from "../../models/shopParams";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseApiUrl = "http://localhost:5097";

  constructor(private http: HttpClient) {}

  getProducts(shopParams: ShopParams): Observable<Pagination<Product[]>> {
    let params = new HttpParams();
    if (shopParams.pageIndex) params = params.append('PageIndex', shopParams.pageIndex.toString());
    if (shopParams.pageSize) params = params.append('PageSize', shopParams.pageSize.toString());
    if (shopParams.categoryId) params = params.append('CategoryID', shopParams.categoryId.toString());
    if (shopParams.sort) params = params.append('sort', shopParams.sort);
    if (shopParams.search) params = params.append('search', shopParams.search.toLowerCase());

    return this.http.get<Pagination<Product[]>>(`${this.baseApiUrl}/Product`, { params });
  }

  getProduct(productId: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseApiUrl}/Product/${productId}`);
  }

}
