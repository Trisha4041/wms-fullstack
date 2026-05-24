import { Component, OnInit, OnDestroy, ChangeDetectorRef, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { EmployeeService } from '../shared/services/employee.service';
import { LeaveService } from '../shared/services/leave.service';
import { ProjectService } from '../shared/services/project.service';
import { DepartmentService } from '../shared/services/department.service';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './admin-dashboard.component.html'
})
export class AdminDashboardComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('leaveChart') leaveChartRef!: ElementRef;
  @ViewChild('deptChart') deptChartRef!: ElementRef;

  totalEmployees = 0;
  totalProjects = 0;
  pendingLeaves = 0;
  totalDepartments = 0;
  username = '';
  deptData: any[] = [];
  chartsReady = false;

  private leaveChartInstance: Chart | null = null;
  private deptChartInstance: Chart | null = null;

  constructor(
    private authService: AuthService,
    private employeeService: EmployeeService,
    private leaveService: LeaveService,
    private projectService: ProjectService,
    private departmentService: DepartmentService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.username = this.authService.getCurrentUser()?.username || '';
    this.loadData();
  }

  ngAfterViewInit() {
    setTimeout(() => {
      if (this.chartsReady) this.renderCharts();
    }, 500);
  }

  ngOnDestroy() {
    if (this.leaveChartInstance) {
      this.leaveChartInstance.destroy();
      this.leaveChartInstance = null;
    }
    if (this.deptChartInstance) {
      this.deptChartInstance.destroy();
      this.deptChartInstance = null;
    }
  }

  loadData() {
    this.employeeService.getAll().subscribe({
      next: (d) => { this.totalEmployees = d.length; this.cdr.detectChanges(); },
      error: () => {}
    });

    this.projectService.getAll().subscribe({
      next: (d) => { this.totalProjects = d.length; this.cdr.detectChanges(); },
      error: () => {}
    });

    this.leaveService.getPending().subscribe({
      next: (d) => { this.pendingLeaves = d.length; this.cdr.detectChanges(); },
      error: () => {}
    });

    this.departmentService.getAll().subscribe({
      next: (d) => {
        this.totalDepartments = d.length;
        this.deptData = d;
        this.chartsReady = true;
        this.cdr.detectChanges();
        setTimeout(() => this.renderCharts(), 300);
      },
      error: () => {}
    });
  }

  renderCharts() {
    this.renderLeaveChart();
    this.renderDeptChart();
  }

  renderLeaveChart() {
    if (!this.leaveChartRef) return;
    if (this.leaveChartInstance) {
      this.leaveChartInstance.destroy();
      this.leaveChartInstance = null;
    }
    const ctx = this.leaveChartRef.nativeElement.getContext('2d');
    this.leaveChartInstance = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['Pending', 'Approved', 'Rejected'],
        datasets: [{
          data: [this.pendingLeaves, 5, 2],
          backgroundColor: ['#F59E0B', '#10B981', '#EF4444'],
          borderWidth: 0
        }]
      },
      options: {
        responsive: true,
        plugins: { legend: { position: 'bottom' } }
      }
    });
  }

  renderDeptChart() {
    if (!this.deptChartRef) return;
    if (this.deptChartInstance) {
      this.deptChartInstance.destroy();
      this.deptChartInstance = null;
    }
    const ctx = this.deptChartRef.nativeElement.getContext('2d');
    this.deptChartInstance = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: this.deptData.map(d => d.departmentName),
        datasets: [{
          label: 'Departments',
          data: this.deptData.map((_, i) => i + 1),
          backgroundColor: ['#3B82F6', '#10B981', '#F59E0B', '#EF4444', '#8B5CF6', '#EC4899'],
          borderRadius: 6
        }]
      },
      options: {
        responsive: true,
        plugins: { legend: { display: false } },
        scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } }
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
