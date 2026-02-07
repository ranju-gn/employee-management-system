import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { EmployeeService } from '../../services/employee.service';
import { DepartmentService } from '../../services/department.service';
import { DesignationService } from '../../services/designation.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    RouterModule,
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  stats = {
    totalEmployees: 0,
    totalDepartments: 0,
    totalDesignations: 0,
    activeEmployees: 0,
  };
  loading = true;

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private designationService: DesignationService,
  ) {}

  ngOnInit(): void {
    this.loadStats();
  }

  loadStats(): void {
    this.loading = true;

    this.employeeService.getEmployees(1, 1000).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.stats.totalEmployees = response.data.totalCount;
          this.stats.activeEmployees = response.data.items.filter(
            (e) => e.isActive,
          ).length;
        }
      },
    });

    this.departmentService.getDepartments().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.stats.totalDepartments = response.data.length;
        }
      },
    });

    this.designationService.getDesignations().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.stats.totalDesignations = response.data.length;
        }
        this.loading = false;
      },
    });
  }
}
