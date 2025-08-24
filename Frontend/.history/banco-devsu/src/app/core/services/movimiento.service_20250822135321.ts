import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Movimiento } from '../models/Movimiento';
import { MovimientoDTO } from '../dtos/movimiento.dto';
import { IMovimientoService } from './interfaces/movimiento-service.interface';

@Injectable({
  providedIn: 'root',
})
export class MovimientoService implements IMovimientoService {
  private apiUrl = `${environment.apiUrl}/Movimientos`;

  constructor(private http: HttpClient) {}

  // Obtener todos los movimientos
  getMovimientos(): Observable<Movimiento[]> {
    return this.http.get<MovimientoD[]>(this.apiUrl);
  }

  // Obtener un movimiento por ID
  getMovimientoById(id: number): Observable<Movimiento> {
    return this.http.get<Movimiento>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo movimiento
  createMovimiento(movimientoDto: MovimientoDTO): Observable<Movimiento> {
    return this.http.post<Movimiento>(this.apiUrl, movimientoDto);
  }

  // Actualizar un movimiento existente
  updateMovimiento(id: number, movimientoDto: MovimientoDTO): Observable<Movimiento> {
    return this.http.put<Movimiento>(`${this.apiUrl}/${id}`, movimientoDto);
  }

  // Eliminar un movimiento
  deleteMovimiento(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
