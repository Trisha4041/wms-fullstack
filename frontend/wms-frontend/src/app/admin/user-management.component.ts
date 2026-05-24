import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-management.component.html'
})
export class UserManagementComponent {
  newUser = { username: '', password: '', roleId: 3 };
  message = '';
  isError = false;

  constructor(private authService: AuthService, private router: Router) {}

  register() {
    this.authService.register(this.newUser).subscribe({
      next: () => {
        this.message = `User "${this.newUser.username}" registered successfully!`;
        this.isError = false;
        this.newUser = { username: '', password: '', roleId: 3 };
      },
      error: (err) => {
        this.message = err.error?.message || 'Registration failed';
        this.isError = true;
      }
    });
  }

  goBack() { this.router.navigate(['/dashboard']); }
}
