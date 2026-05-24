import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DepartmentService } from '../shared/services/department.service';
import { Department } from '../shared/models/department.model';

@Component({
  selector: 'app-department',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './department.component.html'
})
export class DepartmentComponent implements OnInit {
  departments: Department[] = [];
  showForm = false;
  message = '';
  newDepartment = { departmentName: '', description: '' };

  constructor(
    private departmentService: DepartmentService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() { this.loadDepartments(); }

  loadDepartments() {
    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.departments = data;
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }

  createDepartment() {
    this.departmentService.create(this.newDepartment).subscribe({
      next: () => {
        this.message = 'Department created!';
        this.showForm = false;
        this.newDepartment = { departmentName: '', description: '' };
        this.loadDepartments();
      },
      error: (err) => { this.message = err.error?.message || 'Failed'; }
    });
  }

  deleteDepartment(id: number) {
    if (confirm('Delete this department?')) {
      this.departmentService.delete(id).subscribe({
        next: () => this.loadDepartments(),
        error: () => {}
      });
    }
  }

  goBack() { this.router.navigate(['/dashboard']); }
}
