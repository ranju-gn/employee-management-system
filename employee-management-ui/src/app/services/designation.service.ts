import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Designation } from '../models/designation.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root',
})
export class DesignationService {
  private apiUrl = `${environment.apiUrl}/designations`;

  constructor(private http: HttpClient) {}

  getDesignations(): Observable<ApiResponse<Designation[]>> {
    return this.http.get<ApiResponse<Designation[]>>(this.apiUrl);
  }

  getDesignationById(id: number): Observable<ApiResponse<Designation>> {
    return this.http.get<ApiResponse<Designation>>(`${this.apiUrl}/${id}`);
  }

  createDesignation(
    designation: Designation,
  ): Observable<ApiResponse<Designation>> {
    return this.http.post<ApiResponse<Designation>>(this.apiUrl, designation);
  }

  updateDesignation(
    id: number,
    designation: Designation,
  ): Observable<ApiResponse<Designation>> {
    return this.http.put<ApiResponse<Designation>>(
      `${this.apiUrl}/${id}`,
      designation,
    );
  }

  deleteDesignation(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/${id}`);
  }
}
