import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { AttendanceService } from '../shared/services/attendance.service';
import { LeaveService } from '../shared/services/leave.service';
import { EmployeeService } from '../shared/services/employee.service';

@Component({
  selector: 'app-employee-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './employee-dashboard.component.html'
})
export class EmployeeDashboardComponent implements OnInit {
  username = '';
  employeeId = 0;
  todayAttendance: any = null;
  myLeaves: any[] = [];
  myProjects: any[] = [];
  pendingLeaves = 0;

  constructor(
    private authService: AuthService,
    private attendanceService: AttendanceService,
    private leaveService: LeaveService,
    private employeeService: EmployeeService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const user = this.authService.getCurrentUser();
    this.username = user?.username || '';
    this.employeeId = user?.employeeId || 0;

    if (this.employeeId > 0) {
      this.attendanceService.getToday(this.employeeId).subscribe({
        next: (d) => { this.todayAttendance = d; this.cdr.detectChanges(); },
        error: () => {}
      });

      this.leaveService.getByEmployee(this.employeeId).subscribe({
        next: (d) => {
          this.myLeaves = d;
          this.pendingLeaves = d.filter((l: any) => l.status === 'Pending').length;
          this.cdr.detectChanges();
        },
        error: () => {}
      });

      this.employeeService.getMyProjects(this.employeeId).subscribe({
        next: (d) => { this.myProjects = d; this.cdr.detectChanges(); },
        error: () => {}
      });
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
