import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IReportesService } from './interfaces/reportes-service.interface';

@Injectable({
  providedIn: 'root',
})
export class ReportesService implements IReportesService {
  private apiUrl = `${environment.apiUrl}/reportes`;

  constructor(private http: HttpClient) {}

  // Generar reporte de movimientos por cliente
  getReporteMovimientosPorCliente(clienteId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/movimientos/${clienteId}`);
  }

  // Generar reporte de cuentas por cliente
  getReporteCuentasPorCliente(clienteId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/cuentas/${clienteId}`);
  }

  // Generar reporte general
  getReporteGeneral(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/general`);
  }
}
