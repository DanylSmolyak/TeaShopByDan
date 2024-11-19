import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { UserServices } from "../../../shared/services/UserServices";
import { RegisterModel } from "../../../models/UserModels";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  form!: FormGroup;
  errorMessage: string | null = null; // Объявляем переменную для ошибки


  constructor(
    private fb: FormBuilder,
    private registrationService: UserServices
  ) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const model: RegisterModel = this.form.value;

      this.registrationService.register(model).subscribe({
        next: (response: string) => {
          this.errorMessage = response; // Выводим сообщение с бэка
        },
        error: (error) => {
          this.errorMessage = "Registration failed"; // В случае ошибки
        }
      });
    }
  }
  }
