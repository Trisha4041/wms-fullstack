import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  template: `
    <div class="min-h-screen bg-gray-100 flex items-center justify-center">
      <div class="bg-white rounded-xl shadow p-10 text-center max-w-md">
        <div class="text-6xl mb-4">🚫</div>
        <h2 class="text-2xl font-bold text-gray-800 mb-2">Access Denied</h2>
        <p class="text-gray-500 mb-6">You do not have permission to view this page.</p>
        <button (click)="goBack()" class="bg-blue-700 text-white px-6 py-2 rounded-lg hover:bg-blue-800">
          Go Back
        </button>
      </div>
    </div>
  `
})
export class UnauthorizedComponent {
  constructor(private router: Router) {}
  goBack() { this.router.navigate(['/dashboard']); }
}
