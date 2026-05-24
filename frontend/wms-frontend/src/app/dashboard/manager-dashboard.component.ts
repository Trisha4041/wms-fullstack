import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { EmployeeService } from '../shared/services/employee.service';
import { LeaveService } from '../shared/services/leave.service';
import { ProjectService } from '../shared/services/project.service';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './manager-dashboard.component.html'
})
export class ManagerDashboardComponent implements OnInit {
  totalEmployees = 0;
  totalProjects = 0;
  pendingLeaves = 0;
  username = '';

  constructor(
    private authService: AuthService,
    private employeeService: EmployeeService,
    private leaveService: LeaveService,
    private projectService: ProjectService,
    private router: Router
  ) {}

  ngOnInit() {
    this.username = this.authService.getCurrentUser()?.username || '';
    this.employeeService.getAll().subscribe({ next: (d) => this.totalEmployees = d.length, error: () => {} });
    this.projectService.getAll().subscribe({ next: (d) => this.totalProjects = d.length, error: () => {} });
    this.leaveService.getPending().subscribe({ next: (d) => this.pendingLeaves = d.length, error: () => {} });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
