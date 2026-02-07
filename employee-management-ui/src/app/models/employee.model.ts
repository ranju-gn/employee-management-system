export interface Employee {
  id: number;
  employeeCode: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phoneNumber?: string;
  dateOfBirth: Date;
  joiningDate: Date;
  gender: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  departmentId: number;
  departmentName: string;
  designationId: number;
  designationTitle: string;
  reportingManagerId?: number;
  reportingManagerName?: string;
  currentSalary?: number;
  isActive: boolean;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  dateOfBirth: Date;
  joiningDate: Date;
  gender: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  departmentId: number;
  designationId: number;
  reportingManagerId?: number;
}

export interface UpdateEmployeeDto extends CreateEmployeeDto {
  id: number;
  isActive: boolean;
}
