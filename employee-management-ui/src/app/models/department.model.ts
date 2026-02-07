export interface Department {
  id: number;
  name: string;
  code: string;
  description?: string;
  managerId?: number;
  managerName?: string;
  employeeCount: number;
  isActive: boolean;
}

export interface CreateDepartmentDto {
  name: string;
  code: string;
  description?: string;
  managerId?: number;
}
