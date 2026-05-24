import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Leave, ApplyLeave, ApproveLeave } from '../models/leave.model';

@Injectable({
  providedIn: 'root'
})
export class LeaveService {
  private apiUrl = 'http://localhost:5284/api/Leave';

  constructor(private http: HttpClient) {}

  apply(request: ApplyLeave): Observable<Leave> {
    return this.http.post<Leave>(`${this.apiUrl}/apply`, request);
  }

  approveReject(request: ApproveLeave): Observable<Leave> {
    return this.http.put<Leave>(`${this.apiUrl}/approve`, request);
  }

  cancel(leaveId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/cancel/${leaveId}`, {});
  }

  getByEmployee(empId: number): Observable<Leave[]> {
    return this.http.get<Leave[]>(`${this.apiUrl}/employee/${empId}`);
  }

  getPending(): Observable<Leave[]> {
    return this.http.get<Leave[]>(`${this.apiUrl}/pending`);
  }
}
