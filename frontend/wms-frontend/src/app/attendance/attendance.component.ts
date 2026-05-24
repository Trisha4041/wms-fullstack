import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AttendanceService } from '../shared/services/attendance.service';
import { AuthService } from '../shared/services/auth.service';
import { Attendance } from '../shared/models/attendance.model';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './attendance.component.html'
})
export class AttendanceComponent implements OnInit {
  attendance: Attendance[] = [];
  todayAttendance: Attendance | null = null;
  workMode = 'WFO';
  isLoading = false;
  message = '';
  empId = 0;
  hasEmployeeId = false;

  constructor(
    private attendanceService: AttendanceService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const user = this.authService.getCurrentUser();
    this.empId = user?.employeeId || 0;
    this.hasEmployeeId = this.empId > 0;

    if (this.hasEmployeeId) {
      this.loadAttendance();
      this.loadToday();
    }
  }

  loadAttendance() {
    this.attendanceService.getByEmployee(this.empId).subscribe({
      next: (data) => { this.attendance = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  loadToday() {
    this.attendanceService.getToday(this.empId).subscribe({
      next: (data) => { this.todayAttendance = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  checkIn() {
    this.isLoading = true;
    this.attendanceService.checkIn({ empId: this.empId, workMode: this.workMode }).subscribe({
      next: () => {
        this.message = 'Checked in successfully!';
        this.isLoading = false;
        this.loadToday();
        this.loadAttendance();
      },
      error: (err) => {
        this.message = err.error?.message || 'Check-in failed';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  checkOut() {
    this.isLoading = true;
    this.attendanceService.checkOut({ empId: this.empId }).subscribe({
      next: () => {
        this.message = 'Checked out successfully!';
        this.isLoading = false;
        this.loadToday();
        this.loadAttendance();
      },
      error: (err) => {
        this.message = err.error?.message || 'Check-out failed';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  goBack() { this.router.navigate(['/dashboard']); }
}
