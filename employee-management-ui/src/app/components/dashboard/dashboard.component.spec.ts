import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { DashboardComponent } from './dashboard.component';
import { EmployeeService } from '../../services/employee.service';
import { DepartmentService } from '../../services/department.service';
import { DesignationService } from '../../services/designation.service';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let employeeServiceSpy: jasmine.SpyObj<EmployeeService>;
  let departmentServiceSpy: jasmine.SpyObj<DepartmentService>;
  let designationServiceSpy: jasmine.SpyObj<DesignationService>;

  beforeEach(async () => {
    const empSpy = jasmine.createSpyObj('EmployeeService', ['getEmployees']);
    const deptSpy = jasmine.createSpyObj('DepartmentService', [
      'getDepartments',
    ]);
    const desSpy = jasmine.createSpyObj('DesignationService', [
      'getDesignations',
    ]);

    await TestBed.configureTestingModule({
      imports: [DashboardComponent],
      providers: [
        provideRouter([]), // ✅ Add this for routerLink
        provideHttpClient(), // ✅ Add this for HTTP services
        { provide: EmployeeService, useValue: empSpy },
        { provide: DepartmentService, useValue: deptSpy },
        { provide: DesignationService, useValue: desSpy },
      ],
    }).compileComponents();

    employeeServiceSpy = TestBed.inject(
      EmployeeService,
    ) as jasmine.SpyObj<EmployeeService>;
    departmentServiceSpy = TestBed.inject(
      DepartmentService,
    ) as jasmine.SpyObj<DepartmentService>;
    designationServiceSpy = TestBed.inject(
      DesignationService,
    ) as jasmine.SpyObj<DesignationService>;

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load stats on init', () => {
    const mockEmployeeResponse = {
      success: true,
      data: {
        items: [{ isActive: true }, { isActive: false }],
        totalCount: 10,
      },
    };

    const mockDeptResponse = {
      success: true,
      data: [{}, {}, {}],
    };

    const mockDesResponse = {
      success: true,
      data: [{}, {}],
    };

    employeeServiceSpy.getEmployees.and.returnValue(
      of(mockEmployeeResponse as any),
    );
    departmentServiceSpy.getDepartments.and.returnValue(
      of(mockDeptResponse as any),
    );
    designationServiceSpy.getDesignations.and.returnValue(
      of(mockDesResponse as any),
    );

    component.ngOnInit();

    expect(component.stats.totalEmployees).toBe(10);
    expect(component.stats.activeEmployees).toBe(1);
    expect(component.stats.totalDepartments).toBe(3);
    expect(component.stats.totalDesignations).toBe(2);
  });

  it('should initialize with default stats', () => {
    expect(component.stats.totalEmployees).toBe(0);
    expect(component.stats.activeEmployees).toBe(0);
    expect(component.stats.totalDepartments).toBe(0);
    expect(component.stats.totalDesignations).toBe(0);
    expect(component.loading).toBe(true);
  });

  it('should set loading to false after all stats are loaded', () => {
    const mockEmployeeResponse = {
      success: true,
      data: { items: [], totalCount: 0 },
    };
    const mockDeptResponse = { success: true, data: [] };
    const mockDesResponse = { success: true, data: [] };

    employeeServiceSpy.getEmployees.and.returnValue(
      of(mockEmployeeResponse as any),
    );
    departmentServiceSpy.getDepartments.and.returnValue(
      of(mockDeptResponse as any),
    );
    designationServiceSpy.getDesignations.and.returnValue(
      of(mockDesResponse as any),
    );

    component.loadStats();

    expect(component.loading).toBe(false);
  });
});
