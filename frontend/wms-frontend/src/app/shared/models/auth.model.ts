export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  role: string;
  userId: number;
  employeeId: number;
  expiry: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
  roleId: number;
  employeeId?: number;
}
