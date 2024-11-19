import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { UserServices } from "../../../shared/services/UserServices";
import { LoginModel } from "../../../models/UserModels";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  form!: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private userService: UserServices
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]], // Поле email
      password: ['', [Validators.required]] // Поле password
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const model: LoginModel = this.form.value;

      this.userService.login(model).subscribe({
        next: (response: string) => {
          this.errorMessage = null;

        },
        error: (error) => {
          this.errorMessage = error.error || "Login failed";
        }
      });
    }
  }
}
