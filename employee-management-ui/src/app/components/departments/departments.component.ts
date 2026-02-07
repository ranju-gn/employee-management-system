import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { DepartmentService } from '../../services/department.service';
import { Department } from '../../models/department.model';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
  ],
  template: `
    <div class="container">
      <div class="page-header">
        <h1>Departments</h1>
      </div>
      <div class="card-container">
        <table mat-table [dataSource]="departments" class="mat-elevation-z0">
          <ng-container matColumnDef="code">
            <th mat-header-cell *matHeaderCellDef>Code</th>
            <td mat-cell *matCellDef="let dept">{{ dept.code }}</td>
          </ng-container>
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef>Name</th>
            <td mat-cell *matCellDef="let dept">{{ dept.name }}</td>
          </ng-container>
          <ng-container matColumnDef="managerName">
            <th mat-header-cell *matHeaderCellDef>Manager</th>
            <td mat-cell *matCellDef="let dept">
              {{ dept.managerName || 'N/A' }}
            </td>
          </ng-container>
          <ng-container matColumnDef="employeeCount">
            <th mat-header-cell *matHeaderCellDef>Employees</th>
            <td mat-cell *matCellDef="let dept">{{ dept.employeeCount }}</td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
        </table>
      </div>
    </div>
  `,
})
export class DepartmentsComponent implements OnInit {
  departments: Department[] = [];
  displayedColumns = ['code', 'name', 'managerName', 'employeeCount'];

  constructor(private departmentService: DepartmentService) {}

  ngOnInit(): void {
    this.departmentService.getDepartments().subscribe((response) => {
      if (response.success && response.data) {
        this.departments = response.data;
      }
    });
  }
}
