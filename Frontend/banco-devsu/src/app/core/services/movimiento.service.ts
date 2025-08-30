import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Movimiento } from '../models/Movimiento';
import { IMovimientoService } from './interfaces/movimiento-service.interface';
import { MovimientoDto } from '../dtos/movimiento.dto';

@Injectable({
  providedIn: 'root',
})
export class MovimientoService implements IMovimientoService {
  private apiUrl = `${environment.apiUrl}/Movimientos`;

  constructor(private http: HttpClient) {}

  // Obtener todos los movimientos
  getMovimientos(): Observable<MovimientoDto[]> {
    return this.http.get<MovimientoDto[]>(this.apiUrl);
  }

  // Obtener un movimiento por ID
  getMovimientoById(id: number): Observable<Movimiento> {
    return this.http.get<Movimiento>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo movimiento
  createMovimiento(movimientoDto: MovimientoDto): Observable<any> {
    console.log('Service enviando:', movimientoDto);
    return this.http.post<any>(this.apiUrl, movimientoDto);
  }

  // Actualizar un movimiento existente
  updateMovimiento(id: number, movimientoDto: MovimientoDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, movimientoDto);
  }

  // Eliminar un movimiento
  deleteMovimiento(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
