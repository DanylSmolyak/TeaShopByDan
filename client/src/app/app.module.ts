import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './shared/nav-bar/nav-bar.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import { ProductListComponent } from './shared/product-list/product-list.component';
import {ProductService} from "./shared/services/product.service";
import { CategorieComponent } from './shared/categorie/categorie.component';
import { ImgUrlPipe } from './shared/pipes/img-url.pipe';
import {HomeComponent} from "./features/pages/home/home.component";
import { ShoppingCartComponent } from './features/pages/shopping-cart/shopping-cart.component';
import { FooterComponent } from './shared/footer/footer.component';
import { NewsComponent } from './features/news/news.component';
import { LoginComponent } from './features/pages/login/login.component';
import { RegisterComponent } from './features/pages/register/register.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { TeasComponent } from './features/pages/teas/teas.component';
import {MatPaginatorModule} from "@angular/material/paginator";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProductPageComponent } from './features/pages/product-page/product-page.component';
import {CookieService} from "ngx-cookie-service";
import {AuthInterceptor} from "./shared/auth.interceptor";
import { UserPageComponent } from './features/pages/user-page/user-page.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    ProductListComponent,
    CategorieComponent,
    HomeComponent,
    ImgUrlPipe,
    ShoppingCartComponent,
    FooterComponent,
    NewsComponent,
    LoginComponent,
    RegisterComponent,
    TeasComponent,
    ProductPageComponent,
    UserPageComponent
  ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        MatPaginatorModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        FormsModule
    ],
  providers: [ProductService,
    CookieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    } ],
  bootstrap: [AppComponent]
})
export class AppModule { }
