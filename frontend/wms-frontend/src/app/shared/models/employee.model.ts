export interface Employee {
  employeeId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  gender: string;
  dob: string;
  doj: string;
  departmentId: number;
  departmentName: string;
  roleId: number;
  roleName: string;
  status: string;
}

export interface CreateEmployee {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  gender: string;
  dob: string;
  doj: string;
  departmentId: number;
  roleId: number;
}

export interface UpdateEmployee {
  employeeId: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  gender: string;
  departmentId: number;
  roleId: number;
  status: string;
}
