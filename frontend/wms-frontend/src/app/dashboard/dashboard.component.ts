import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: ''
})
export class DashboardComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    const role = this.authService.getRole();
    if (role === 'Admin') {
      this.router.navigate(['/admin/dashboard']);
    } else if (role === 'Manager') {
      this.router.navigate(['/manager/dashboard']);
    } else {
      this.router.navigate(['/employee/dashboard']);
    }
  }
}
