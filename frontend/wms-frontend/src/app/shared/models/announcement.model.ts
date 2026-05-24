export interface Announcement {
  announcementId: number;
  title: string;
  message: string;
  createdBy: number;
  createdOn: string;
  isActive: boolean;
}

export interface CreateAnnouncement {
  title: string;
  message: string;
}
