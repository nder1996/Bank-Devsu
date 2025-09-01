import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReporteEstadoCuentaResponseDto, ReporteEstadoCuentaRequestDto } from '../dtos/reporte.dto';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReporteService {
  private apiUrl = `${environment.apiUrl}/Reportes`;

  constructor(private http: HttpClient) { }

  /**
   * Generar reporte de estado de cuenta especificando un rango de fechas y un cliente
   * GET /api/reportes?clienteId=1&fechaInicio=2024-01-01&fechaFin=2024-12-31&formato=json
   */
  generarEstadoCuenta(
    clienteId: number,
    fechaInicio: Date,
    fechaFin: Date,
    formato: string = 'json'
  ): Observable<any> {
    let params = new HttpParams()
      .set('clienteId', clienteId.toString())
      .set('fechaInicio', fechaInicio.toISOString().split('T')[0])
      .set('fechaFin', fechaFin.toISOString().split('T')[0])
      .set('formato', formato);

    return this.http.get(`${this.apiUrl}`, { params });
  }

  /**
   * Descargar reporte de estado de cuenta en formato PDF
   * GET /api/reportes/pdf?clienteId=1&fechaInicio=2024-01-01&fechaFin=2024-12-31
   */
  descargarEstadoCuentaPdf(
    clienteId: number,
    fechaInicio: Date,
    fechaFin: Date
  ): Observable<Blob> {
    let params = new HttpParams()
      .set('clienteId', clienteId.toString())
      .set('fechaInicio', fechaInicio.toISOString().split('T')[0])
      .set('fechaFin', fechaFin.toISOString().split('T')[0]);

    return this.http.get(`${this.apiUrl}/pdf`, {
      params,
      responseType: 'blob'
    });
  }

  /**
   * Descargar reporte de estado de cuenta en formato JSON
   * GET /api/reportes/json?clienteId=1&fechaInicio=2024-01-01&fechaFin=2024-12-31
   */
  descargarEstadoCuentaJson(
    clienteId: number,
    fechaInicio: Date,
    fechaFin: Date
  ): Observable<Blob> {
    let params = new HttpParams()
      .set('clienteId', clienteId.toString())
      .set('fechaInicio', fechaInicio.toISOString().split('T')[0])
      .set('fechaFin', fechaFin.toISOString().split('T')[0]);

    return this.http.get(`${this.apiUrl}/json`, {
      params,
      responseType: 'blob'
    });
  }
}