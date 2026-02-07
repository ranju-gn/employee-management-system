import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  MatNativeDateModule,
  provideNativeDateAdapter,
} from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { EmployeeService } from '../../../services/employee.service';
import { DepartmentService } from '../../../services/department.service';
import { DesignationService } from '../../../services/designation.service';
import { Employee } from '../../../models/employee.model';
import { Department } from '../../../models/department.model';
import { Designation } from '../../../models/designation.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSlideToggleModule,
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss'],
})
export class EmployeeFormComponent implements OnInit {
  employeeForm: FormGroup;
  loading = false;
  mode: 'create' | 'edit';
  departments: Department[] = [];
  designations: Designation[] = [];
  managers: Employee[] = [];
  maxDate = new Date();

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private designationService: DesignationService,
    private dialogRef: MatDialogRef<EmployeeFormComponent>,
    private toastr: ToastrService,
    @Inject(MAT_DIALOG_DATA)
    public data: { mode: 'create' | 'edit'; employee?: Employee },
  ) {
    this.mode = data.mode;
    this.employeeForm = this.createForm();
  }

  ngOnInit(): void {
    this.loadDepartments();
    this.loadDesignations();
    this.loadManagers();

    if (this.mode === 'edit' && this.data.employee) {
      this.patchFormValues(this.data.employee);
    }
  }

  createForm(): FormGroup {
    return this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.pattern(/^[0-9]{10}$/)]],
      dateOfBirth: ['', Validators.required],
      joiningDate: ['', Validators.required],
      gender: ['', Validators.required],
      address: [''],
      city: [''],
      state: [''],
      country: [''],
      postalCode: [''],
      departmentId: ['', Validators.required],
      designationId: ['', Validators.required],
      reportingManagerId: [''],
      isActive: [true],
    });
  }

  patchFormValues(employee: Employee): void {
    this.employeeForm.patchValue({
      firstName: employee.firstName,
      lastName: employee.lastName,
      email: employee.email,
      phoneNumber: employee.phoneNumber,
      dateOfBirth: new Date(employee.dateOfBirth),
      joiningDate: new Date(employee.joiningDate),
      gender: employee.gender,
      address: employee.address,
      city: employee.city,
      state: employee.state,
      country: employee.country,
      postalCode: employee.postalCode,
      departmentId: employee.departmentId,
      designationId: employee.designationId,
      reportingManagerId: employee.reportingManagerId,
      isActive: employee.isActive,
    });
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.departments = response.data.filter((d) => d.isActive);
        }
      },
    });
  }

  loadDesignations(): void {
    this.designationService.getDesignations().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.designations = response.data.filter((d) => d.isActive);
        }
      },
    });
  }

  loadManagers(): void {
    this.employeeService.getEmployees(1, 1000).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.managers = response.data.items.filter((e) => e.isActive);
        }
      },
    });
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    const formData = this.employeeForm.value;

    if (this.mode === 'create') {
      this.employeeService.createEmployee(formData).subscribe({
        next: (response) => {
          if (response.success) {
            this.toastr.success('Employee created successfully', 'Success');
            this.dialogRef.close(true);
          }
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        },
      });
    } else {
      const updateData = { ...formData, id: this.data.employee!.id };
      this.employeeService
        .updateEmployee(this.data.employee!.id, updateData)
        .subscribe({
          next: (response) => {
            if (response.success) {
              this.toastr.success('Employee updated successfully', 'Success');
              this.dialogRef.close(true);
            }
            this.loading = false;
          },
          error: () => {
            this.loading = false;
          },
        });
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
