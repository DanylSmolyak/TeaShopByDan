import {Component, OnInit} from '@angular/core';
import {UserServices} from "../../../shared/services/UserServices";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {User} from "../../../models/UserModels";

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent implements OnInit {
  profileForm: FormGroup;
  profileImage: string | null = null;
  userEmail: string = '';
  successMessage: string = '';
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private userService: UserServices
  ) {
    this.profileForm = this.fb.group({
      username: ['', Validators.required],
      address: ['']
    });
  }

  ngOnInit() {
    this.loadUserProfile();
  }

  loadUserProfile() {
    this.userService.getUserProfile().subscribe({
      next: (profile) => {
        this.profileForm.patchValue({
          username: profile.username,
          address: profile.address
        });
        this.userEmail = profile.email;
        this.profileImage = profile.image || null;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load profile data';
        console.error('Error loading profile:', error);
      }
    });
  }

  onSubmit() {
    if (this.profileForm.valid) {
      this.userService.updateProfile(this.profileForm.value).subscribe({
        next: () => {
          this.successMessage = 'Profile updated successfully';
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.errorMessage = 'Failed to update profile';
          console.error('Error updating profile:', error);
        }
      });
    }
  }

  onImageUpload(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.userService.uploadProfileImage(file).subscribe({
        next: (response) => {
          this.profileImage = response.imageUrl;
          this.successMessage = 'Profile image updated successfully';
          setTimeout(() => this.successMessage = '', 3000);
        },
        error: (error) => {
          this.errorMessage = 'Failed to upload image';
          console.error('Error uploading image:', error);
        }
      });
    }
  }
}
