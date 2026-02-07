import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { DesignationService } from '../../services/designation.service';
import { Designation } from '../../models/designation.model';

@Component({
  selector: 'app-designations',
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
        <h1>Designations</h1>
      </div>
      <div class="card-container">
        <table mat-table [dataSource]="designations" class="mat-elevation-z0">
          <ng-container matColumnDef="code">
            <th mat-header-cell *matHeaderCellDef>Code</th>
            <td mat-cell *matCellDef="let des">{{ des.code }}</td>
          </ng-container>
          <ng-container matColumnDef="title">
            <th mat-header-cell *matHeaderCellDef>Title</th>
            <td mat-cell *matCellDef="let des">{{ des.title }}</td>
          </ng-container>
          <ng-container matColumnDef="level">
            <th mat-header-cell *matHeaderCellDef>Level</th>
            <td mat-cell *matCellDef="let des">{{ des.level }}</td>
          </ng-container>
          <ng-container matColumnDef="employeeCount">
            <th mat-header-cell *matHeaderCellDef>Employees</th>
            <td mat-cell *matCellDef="let des">{{ des.employeeCount }}</td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
        </table>
      </div>
    </div>
  `,
})
export class DesignationsComponent implements OnInit {
  designations: Designation[] = [];
  displayedColumns = ['code', 'title', 'level', 'employeeCount'];

  constructor(private designationService: DesignationService) {}

  ngOnInit(): void {
    this.designationService.getDesignations().subscribe((response) => {
      if (response.success && response.data) {
        this.designations = response.data;
      }
    });
  }
}
