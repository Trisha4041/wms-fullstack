import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Attendance, CheckInRequest, CheckOutRequest } from '../models/attendance.model';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
  private apiUrl = 'http://localhost:5284/api/Attendance';

  constructor(private http: HttpClient) {}

  checkIn(request: CheckInRequest): Observable<Attendance> {
    return this.http.post<Attendance>(`${this.apiUrl}/checkin`, request);
  }

  checkOut(request: CheckOutRequest): Observable<Attendance> {
    return this.http.post<Attendance>(`${this.apiUrl}/checkout`, request);
  }

  getByEmployee(empId: number): Observable<Attendance[]> {
    return this.http.get<Attendance[]>(`${this.apiUrl}/employee/${empId}`);
  }

  getToday(empId: number): Observable<Attendance> {
    return this.http.get<Attendance>(`${this.apiUrl}/today/${empId}`);
  }
}
