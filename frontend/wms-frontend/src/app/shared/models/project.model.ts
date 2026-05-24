export interface Project {
  projectId: number;
  projectName: string;
  clientId: number | null;
  clientName: string;
  startDate: string;
  endDate: string;
  status: string;
}

export interface CreateProject {
  projectName: string;
  clientId: number | null;
  startDate: string | null;
  endDate: string | null;
}

export interface AssignEmployee {
  empId: number;
  projectId: number;
  assignedBy: string;
}
