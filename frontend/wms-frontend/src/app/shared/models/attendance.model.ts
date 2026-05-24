export interface Attendance {
  attendanceId: number;
  empId: number;
  employeeName: string;
  checkIn: string;
  checkOut: string;
  totalHours: number;
  workMode: string;
  attendanceDate: string;
}

export interface CheckInRequest {
  empId: number;
  workMode: string;
}

export interface CheckOutRequest {
  empId: number;
}
