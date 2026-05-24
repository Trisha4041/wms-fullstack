import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ProjectService } from '../shared/services/project.service';
import { EmployeeService } from '../shared/services/employee.service';
import { AuthService } from '../shared/services/auth.service';
import { Project } from '../shared/models/project.model';
import { Employee } from '../shared/models/employee.model';

@Component({
  selector: 'app-project',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './project.component.html'
})
export class ProjectComponent implements OnInit {
  projects: Project[] = [];
  employees: Employee[] = [];
  assignedEmployees: any[] = [];
  showForm = false;
  showAssignForm = false;
  showAssignedList = false;
  selectedProjectId = 0;
  selectedProjectName = '';
  message = '';
  username = '';
  newProject = { projectName: '', startDate: '', endDate: '' };
  assignData = { empId: 0, projectId: 0, assignedBy: '' };

  constructor(
    private projectService: ProjectService,
    private employeeService: EmployeeService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.username = this.authService.getCurrentUser()?.username || '';
    this.loadProjects();
    this.loadEmployees();
  }

  loadProjects() {
    this.projectService.getAll().subscribe({
      next: (data) => { this.projects = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  loadEmployees() {
    this.employeeService.getAll().subscribe({
      next: (data) => { this.employees = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  createProject() {
    if (!this.newProject.projectName) {
      this.message = 'Please enter project name';
      return;
    }
    const payload = {
      projectName: this.newProject.projectName,
      clientId: null,
      startDate: this.newProject.startDate || null,
      endDate: this.newProject.endDate || null
    };
    this.projectService.create(payload).subscribe({
      next: () => {
        this.message = 'Project created!';
        this.showForm = false;
        this.newProject = { projectName: '', startDate: '', endDate: '' };
        this.loadProjects();
      },
      error: (err) => { this.message = err.error?.message || 'Failed'; }
    });
  }

  openAssign(project: Project) {
    this.selectedProjectId = project.projectId;
    this.selectedProjectName = project.projectName;
    this.assignData = { empId: 0, projectId: project.projectId, assignedBy: this.username };
    this.showAssignForm = true;
    this.showAssignedList = false;
    this.cdr.detectChanges();
  }

  viewAssigned(project: Project) {
    this.selectedProjectId = project.projectId;
    this.selectedProjectName = project.projectName;
    this.showAssignedList = true;
    this.showAssignForm = false;
    this.projectService.getAssignedEmployees(project.projectId).subscribe({
      next: (data) => { this.assignedEmployees = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  assignEmployee() {
    if (!this.assignData.empId) {
      this.message = 'Please select an employee';
      return;
    }
    this.projectService.assignEmployee(this.assignData).subscribe({
      next: () => {
        this.message = `Employee assigned to ${this.selectedProjectName} successfully!`;
        this.showAssignForm = false;
        this.cdr.detectChanges();
      },
      error: (err) => { this.message = err.error?.message || 'Failed to assign'; }
    });
  }

  deleteProject(id: number) {
    if (confirm('Delete this project?')) {
      this.projectService.delete(id).subscribe({
        next: () => this.loadProjects(),
        error: () => {}
      });
    }
  }

  goBack() { this.router.navigate(['/dashboard']); }
}
