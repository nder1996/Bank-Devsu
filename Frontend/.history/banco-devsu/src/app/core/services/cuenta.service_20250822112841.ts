import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Cuenta } from '../models/Cuenta';
import { CuentaDTO } from '../dtos/cuenta.dto';


@Injectable({
  providedIn: 'root',
})
export class CuentaService {
  private apiUrl = `${environment.apiUrl}/cuentas`;

  constructor(private http: HttpClient) {}

  // Obtener todas las cuentas
  getCuentas(): Observable<Cuenta[]> {
    return this.http.get<Cuenta[]>(this.apiUrl);
  }

  // Obtener una cuenta por ID
  getCuentaById(id: number): Observable<Cuenta> {
    return this.http.get<Cuenta>(`${this.apiUrl}/${id}`);
  }

  // Crear una nueva cuenta
  createCuenta(cuentaDto: CuentaDTO): Observable<Cuenta> {
    return this.http.post<Cuenta>(this.apiUrl, cuentaDto);
  }

  // Actualizar una cuenta existente
  updateCuenta(id: number, cuentaDto: CuentaDTO): Observable<Cuenta> {
    return this.http.put<Cuenta>(`${this.apiUrl}/${id}`, cuentaDto);
  }

  // Eliminar una cuenta
  deleteCuenta(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
