  import {inject, Injectable} from '@angular/core';
  import {HttpClient, HttpErrorResponse} from "@angular/common/http";
  import {LoginModel, RegisterModel, UpdateUserProfile, User} from "../../models/UserModels";
  import {catchError, Observable, tap, throwError} from "rxjs";
  import {CookieService} from "ngx-cookie-service";
  import {Router} from "@angular/router";

  @Injectable({
    providedIn: 'root'
  })
  export class UserServices {
    constructor(
      private http: HttpClient,
      private cookieService: CookieService,
      private router: Router
    ) {}

    baseApiUrl = "http://localhost:5097";

    register(model: RegisterModel): Observable<any> {
      return this.http.post(`${this.baseApiUrl}/user/register`, model, { responseType: 'text' })
    }

    login(model: { email: string; password: string }) {
      return this.http.post<any>(`${this.baseApiUrl}/user/token`, model).pipe(
        tap(response => {
          if (response.token) {
            // Сохраняем токен и данные пользователя в куки
            this.cookieService.set('token', response.token);
            this.cookieService.set('user', JSON.stringify({
              email: response.email,
              username: response.userName,
              roles: response.roles
            }));
            this.router.navigate(['/']);
          }
        })
      );
    }

    logout() {
      this.cookieService.delete('token');
      this.cookieService.delete('user');
      this.router.navigate(['/login']);
    }

    isLoggedIn(): boolean {
      return this.cookieService.check('token');
    }

    getToken(): string {
      return this.cookieService.get('token');
    }

    getUser() {
      const userData = this.cookieService.get('user');
      return userData ? JSON.parse(userData) : null;
    }
    getUserProfile(): Observable<User> {
      const userId = this.getUser()?.id;
      return this.http.get<User>(`${this.baseApiUrl}/user`);
    }

    updateProfile(profileData: UpdateUserProfile): Observable<any> {
      return this.http.post(`${this.baseApiUrl}/user/update-profile`, profileData);
    }

    uploadProfileImage(file: File): Observable<any> {
      const formData = new FormData();
      formData.append('file', file);
      return this.http.post(`${this.baseApiUrl}/user/upload-image`, formData);
    }
  }
