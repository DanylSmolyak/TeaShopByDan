import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {Product} from "../../models/product";
import {Pagination} from "../../models/pagination";

@Injectable({
  providedIn: 'root'
})
export class ProductPageService {
  http = inject(HttpClient);
  baseApiUrl = "http://localhost:5097";

  getProduct(productId: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseApiUrl}/Product/${productId}`);
  }
}
