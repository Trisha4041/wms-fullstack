import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LeaveService } from '../shared/services/leave.service';
import { AuthService } from '../shared/services/auth.service';
import { Leave } from '../shared/models/leave.model';

@Component({
  selector: 'app-leave',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './leave.component.html'
})
export class LeaveComponent implements OnInit {
  leaves: Leave[] = [];
  pendingLeaves: Leave[] = [];
  showForm = false;
  message = '';
  empId = 0;
  role = '';

  newLeave = {
    empId: 0, leaveType: '', reason: '', fromDate: '', toDate: ''
  };

  constructor(
    private leaveService: LeaveService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const user = this.authService.getCurrentUser();
    this.role = user?.role || '';
    this.empId = user?.employeeId || user?.userId || 0;
    this.newLeave.empId = this.empId;
    this.loadLeaves();
    if (this.role === 'Admin' || this.role === 'Manager') {
      this.loadPending();
    }
  }

  loadLeaves() {
    this.leaveService.getByEmployee(this.empId).subscribe({
      next: (data) => { this.leaves = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  loadPending() {
    this.leaveService.getPending().subscribe({
      next: (data) => { this.pendingLeaves = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  applyLeave() {
    if (!this.newLeave.leaveType || !this.newLeave.fromDate || !this.newLeave.toDate) {
      this.message = 'Please fill all required fields';
      return;
    }
    this.leaveService.apply(this.newLeave).subscribe({
      next: () => {
        this.message = 'Leave applied successfully!';
        this.showForm = false;
        this.loadLeaves();
        this.cdr.detectChanges();
      },
      error: (err) => { this.message = err.error?.message || 'Failed to apply leave'; }
    });
  }

  approveLeave(leaveId: number, status: string) {
    this.leaveService.approveReject({ leaveId, status, approvedBy: this.empId }).subscribe({
      next: () => {
        this.message = `Leave ${status} successfully!`;
        this.loadPending();
        this.loadLeaves();
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  cancelLeave(leaveId: number) {
    this.leaveService.cancel(leaveId).subscribe({
      next: () => { this.message = 'Leave cancelled'; this.loadLeaves(); },
      error: () => {}
    });
  }

  goBack() { this.router.navigate(['/dashboard']); }
}
