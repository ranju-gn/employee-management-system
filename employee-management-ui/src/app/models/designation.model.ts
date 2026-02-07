export interface Designation {
  id: number;
  title: string;
  code: string;
  description?: string;
  level: number;
  employeeCount: number;
  isActive: boolean;
}
