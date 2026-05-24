import { Routes } from '@angular/router';
import { authGuard, adminGuard, managerGuard } from './shared/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./auth/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./auth/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'unauthorized',
    loadComponent: () => import('./shared/unauthorized.component').then(m => m.UnauthorizedComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'admin/dashboard',
    loadComponent: () => import('./dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent),
    canActivate: [adminGuard]
  },
  {
    path: 'manager/dashboard',
    loadComponent: () => import('./dashboard/manager-dashboard.component').then(m => m.ManagerDashboardComponent),
    canActivate: [managerGuard]
  },
  {
    path: 'employee/dashboard',
    loadComponent: () => import('./dashboard/employee-dashboard.component').then(m => m.EmployeeDashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'admin/users',
    loadComponent: () => import('./admin/user-management.component').then(m => m.UserManagementComponent),
    canActivate: [adminGuard]
  },
  {
    path: 'employees',
    loadComponent: () => import('./employees/employee-list.component').then(m => m.EmployeeListComponent),
    canActivate: [managerGuard]
  },
  {
    path: 'attendance',
    loadComponent: () => import('./attendance/attendance.component').then(m => m.AttendanceComponent),
    canActivate: [authGuard]
  },
  {
    path: 'leaves',
    loadComponent: () => import('./leaves/leave.component').then(m => m.LeaveComponent),
    canActivate: [authGuard]
  },
  {
    path: 'projects',
    loadComponent: () => import('./projects/project.component').then(m => m.ProjectComponent),
    canActivate: [managerGuard]
  },
  {
    path: 'departments',
    loadComponent: () => import('./departments/department.component').then(m => m.DepartmentComponent),
    canActivate: [adminGuard]
  },
  {
    path: 'announcements',
    loadComponent: () => import('./announcements/announcement.component').then(m => m.AnnouncementComponent),
    canActivate: [authGuard]
  },
  { path: '**', redirectTo: 'login' }
];
