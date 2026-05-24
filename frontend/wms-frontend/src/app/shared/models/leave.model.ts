export interface Leave {
  leaveId: number;
  empId: number;
  employeeName: string;
  leaveType: string;
  reason: string;
  fromDate: string;
  toDate: string;
  status: string;
  appliedOn: string;
  approvedBy: number;
  approvedOn: string;
}

export interface ApplyLeave {
  empId: number;
  leaveType: string;
  reason: string;
  fromDate: string;
  toDate: string;
}

export interface ApproveLeave {
  leaveId: number;
  status: string;
  approvedBy: number;
}
