import { Observable } from 'rxjs';

export interface IReportesService {
  getReporteMovimientosPorCliente(clienteId: number): Observable<any>;
  getReporteCuentasPorCliente(clienteId: number): Observable<any>;
  getReporteGeneral(): Observable<any>;
}
