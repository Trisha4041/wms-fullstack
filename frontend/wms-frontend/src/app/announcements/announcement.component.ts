import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AnnouncementService } from '../shared/services/announcement.service';
import { AuthService } from '../shared/services/auth.service';
import { Announcement } from '../shared/models/announcement.model';

@Component({
  selector: 'app-announcement',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './announcement.component.html'
})
export class AnnouncementComponent implements OnInit {
  announcements: Announcement[] = [];
  showForm = false;
  message = '';
  role = '';
  newAnnouncement = { title: '', message: '' };

  constructor(
    private announcementService: AnnouncementService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.role = this.authService.getRole();
    this.loadAnnouncements();
  }

  loadAnnouncements() {
    this.announcementService.getAll().subscribe({
      next: (data) => { this.announcements = data; this.cdr.detectChanges(); },
      error: () => {}
    });
  }

  create() {
    if (!this.newAnnouncement.title || !this.newAnnouncement.message) {
      this.message = 'Please fill all fields';
      return;
    }
    this.announcementService.create(this.newAnnouncement).subscribe({
      next: () => {
        this.message = 'Announcement posted!';
        this.showForm = false;
        this.newAnnouncement = { title: '', message: '' };
        this.loadAnnouncements();
      },
      error: (err) => { this.message = err.error?.message || 'Failed'; }
    });
  }

  delete(id: number) {
    if (confirm('Delete this announcement?')) {
      this.announcementService.delete(id).subscribe({
        next: () => { this.message = 'Deleted!'; this.loadAnnouncements(); },
        error: () => {}
      });
    }
  }

  goBack() { this.router.navigate(['/dashboard']); }
}
