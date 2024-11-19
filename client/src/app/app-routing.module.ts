import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from "./features/pages/home/home.component";
import {RegisterComponent} from "./features/pages/register/register.component";
import {LoginComponent} from "./features/pages/login/login.component";
import {TeasComponent} from "./features/pages/teas/teas.component";
import {ProductPageComponent} from "./features/pages/product-page/product-page.component";
import {ShoppingCartComponent} from "./features/pages/shopping-cart/shopping-cart.component";
import {UserPageComponent} from "./features/pages/user-page/user-page.component";

const routes: Routes = [
  // Основные маршруты
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '/home'
  },
  { path: 'profile', component: UserPageComponent },
  {
    path: 'home',
    component: HomeComponent,
    title: 'Главная' // Для title сервиса
  },
  { path: 'cart', component: ShoppingCartComponent },
  // Маршруты пользователя
  {
    path: 'user',
    children: [
      {
        path: 'register',
        component: RegisterComponent,
        title: 'Регистрация'
      },
      {
        path: 'login',
        component: LoginComponent,
        title: 'Вход'
      }
    ]
  },

  // Маршруты продуктов
  {
    path: 'teas',
    component: TeasComponent,
    title: 'Каталог чая'
  },
  {
    path: 'product/:id',
    component: ProductPageComponent,
    title: 'Детали продукта'
  },

  // Маршрут для обработки несуществующих URL

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'enabled', // Автоматическая прокрутка вверх при навигации
      anchorScrolling: 'enabled', // Поддержка якорных ссылок
      scrollOffset: [0, 64] // Отступ при прокрутке (полезно при фиксированном header)
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
