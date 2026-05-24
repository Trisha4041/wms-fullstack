export interface Department {
  departmentId: number;
  departmentName: string;
  description: string;
  createdOn: string;
}

export interface CreateDepartment {
  departmentName: string;
  description: string;
}
