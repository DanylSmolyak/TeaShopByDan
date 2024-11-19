import {Component, inject, OnInit} from '@angular/core';
import {UserServices} from "../services/UserServices";
import {Router} from "@angular/router";
import {debounceTime, distinctUntilChanged, Subject} from "rxjs";
import {ProductService} from "../services/product.service";


@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  isLoggedIn: boolean = false;
  cartItemsCount: number = 0;
  currentUser: any = null;
  searchTerm: string = '';
  profile:  any = null;
  private searchSubject = new Subject<string>()

  constructor(
    private userService: UserServices,
    private router: Router,
    private productService: ProductService
  ) {
    this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchTerm => {
        this.router.navigate(['/teas'], {
          queryParams: { search: searchTerm }
        });
      });
  }

  ngOnInit() {
    this.checkAuthStatus();
    this.loadUserData();
  }

  private checkAuthStatus() {
    this.isLoggedIn = this.userService.isLoggedIn();
  }

  private loadUserData() {
    if (this.isLoggedIn) {
      this.currentUser = this.userService.getUser();
      this.userService.getUserProfile().subscribe(profile => {
        this.profile = profile;
        console.log('User profile:', this.profile);
      });
    }
  }

  logout() {
    this.userService.logout();
    this.isLoggedIn = false;
    this.currentUser = null;
  }

  // Вспомогательные методы для работы с пользователем
  getUserName(): string {
    return this.currentUser?.username || 'Гость';
  }

  getUserImage(): string {
    console.log('User image path:', this.currentUser?.image);  // Log image path
    return this.profile?.image || 'Гость';
  }

  hasRole(role: string): boolean {
    return this.currentUser?.roles?.includes(role) || false;
  }

  onSearchInput(event: any) {
    this.searchSubject.next(this.searchTerm);
  }
}
