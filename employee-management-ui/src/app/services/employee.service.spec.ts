import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { EmployeeService } from './employee.service';
import { Employee, CreateEmployeeDto } from '../models/employee.model';
import { environment } from '../../environments/environment';

describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EmployeeService],
    });

    service = TestBed.inject(EmployeeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getEmployees', () => {
    it('should fetch employees with pagination', () => {
      const mockResponse = {
        success: true,
        data: {
          items: [],
          pageNumber: 1,
          pageSize: 10,
          totalCount: 0,
          totalPages: 0,
          hasPreviousPage: false,
          hasNextPage: false,
        },
      };

      service.getEmployees(1, 10).subscribe((response) => {
        expect(response.success).toBe(true);
        expect(response.data?.items).toEqual([]);
      });

      const req = httpMock.expectOne(
        `${environment.apiUrl}/employees?pageNumber=1&pageSize=10`,
      );
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });

    it('should include search term in request', () => {
      const searchTerm = 'John';
      const mockResponse = { success: true, data: { items: [] } };

      service.getEmployees(1, 10, searchTerm).subscribe();

      const req = httpMock.expectOne(
        `${environment.apiUrl}/employees?pageNumber=1&pageSize=10&searchTerm=${searchTerm}`,
      );
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });
  });

  describe('getEmployeeById', () => {
    it('should fetch single employee', () => {
      const mockEmployee: Employee = {
        id: 1,
        employeeCode: 'EMP001',
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        email: 'john@test.com',
        dateOfBirth: new Date(),
        joiningDate: new Date(),
        gender: 'Male',
        departmentId: 1,
        departmentName: 'IT',
        designationId: 1,
        designationTitle: 'Developer',
        isActive: true,
      };

      const mockResponse = { success: true, data: mockEmployee };

      service.getEmployeeById(1).subscribe((response) => {
        expect(response.data).toEqual(mockEmployee);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/employees/1`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });
  });

  describe('createEmployee', () => {
    it('should create new employee', () => {
      const newEmployee: CreateEmployeeDto = {
        firstName: 'Jane',
        lastName: 'Doe',
        email: 'jane@test.com',
        dateOfBirth: new Date(),
        joiningDate: new Date(),
        gender: 'Female',
        departmentId: 1,
        designationId: 1,
      };

      const mockResponse = { success: true, data: { id: 1, ...newEmployee } };

      service.createEmployee(newEmployee).subscribe((response) => {
        expect(response.success).toBe(true);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/employees`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newEmployee);
      req.flush(mockResponse);
    });
  });

  describe('deleteEmployee', () => {
    it('should delete employee', () => {
      const mockResponse = { success: true, data: true };

      service.deleteEmployee(1).subscribe((response) => {
        expect(response.success).toBe(true);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/employees/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush(mockResponse);
    });
  });
});
