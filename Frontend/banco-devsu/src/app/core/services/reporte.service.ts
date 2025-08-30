import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReporteDto, ReporteRequestDto } from '../dtos/reporte.dto';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReporteService {
  private apiUrl = `${environment.apiUrl}/Reportes`;

  constructor(private http: HttpClient) { }

  getReportes(): Observable<any> {
    return this.http.get(`${this.apiUrl}`);
  }

  getReporte(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  getReportesByCliente(clienteId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/cliente/${clienteId}`);
  }

  createReporte(reporte: ReporteRequestDto): Observable<any> {
    return this.http.post(`${this.apiUrl}`, reporte);
  }

  updateReporte(id: number, reporte: ReporteDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, reporte);
  }

  deleteReporte(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  downloadReporte(id: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${id}/download`, {
      responseType: 'blob'
    });
  }
}