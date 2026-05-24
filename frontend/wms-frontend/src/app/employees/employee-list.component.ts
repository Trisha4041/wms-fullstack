import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from '../shared/services/employee.service';
import { DepartmentService } from '../shared/services/department.service';
import { Employee } from '../shared/models/employee.model';
import { Department } from '../shared/models/department.model';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './employee-list.component.html'
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  filteredEmployees: Employee[] = [];
  departments: Department[] = [];
  isLoading = true;
  hasError = false;
  showForm = false;
  errorMessage = '';
  searchName = '';
  searchDepartment = 0;
  successInfo: any = null;

  newEmployee = {
    firstName: '', lastName: '', email: '',
    phoneNumber: '', gender: '', dob: '',
    doj: '', departmentId: 0, roleId: 3
  };

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadEmployees();
    this.loadDepartments();
  }

  loadEmployees() {
    this.isLoading = true;
    this.hasError = false;
    this.employeeService.getAll().subscribe({
      next: (data) => {
        this.employees = data;
        this.filteredEmployees = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.hasError = true;
        this.cdr.detectChanges();
      }
    });
  }

  loadDepartments() {
    this.departmentService.getAll().subscribe({
      next: (data) => { this.departments = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  search() {
    this.filteredEmployees = this.employees.filter(emp => {
      const nameMatch = this.searchName === '' ||
        (emp.firstName + ' ' + emp.lastName).toLowerCase().includes(this.searchName.toLowerCase());
      const deptMatch = this.searchDepartment === 0 ||
        emp.departmentId === Number(this.searchDepartment);
      return nameMatch && deptMatch;
    });
    this.cdr.detectChanges();
  }

  clearSearch() {
    this.searchName = '';
    this.searchDepartment = 0;
    this.filteredEmployees = this.employees;
    this.cdr.detectChanges();
  }

  createEmployee() {
    if (!this.newEmployee.firstName || !this.newEmployee.email ||
        !this.newEmployee.departmentId || !this.newEmployee.dob || !this.newEmployee.doj) {
      this.errorMessage = 'Please fill all required fields';
      return;
    }
    this.employeeService.create(this.newEmployee).subscribe({
      next: (emp) => {
        this.showForm = false;
        const username = (this.newEmployee.firstName + '.' + this.newEmployee.lastName).toLowerCase().replace(' ', '');
        this.successInfo = {
          name: this.newEmployee.firstName + ' ' + this.newEmployee.lastName,
          username: username,
          password: 'WMS@1234'
        };
        this.loadEmployees();
        this.resetForm();
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to create employee';
      }
    });
  }

  deleteEmployee(id: number) {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.delete(id).subscribe({
        next: () => this.loadEmployees(),
        error: () => {}
      });
    }
  }

  resetForm() {
    this.newEmployee = {
      firstName: '', lastName: '', email: '',
      phoneNumber: '', gender: '', dob: '',
      doj: '', departmentId: 0, roleId: 3
    };
    this.errorMessage = '';
  }

  goBack() { this.router.navigate(['/dashboard']); }
}
