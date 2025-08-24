import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Cliente } from '../models/Cliente';
import { IClienteService } from './interfaces/cliente-service.interface';
import { ClienteDto } from '../dtos/cliente.dto';

@Injectable({
  providedIn: 'root',
})
export class ClienteService implements IClienteService {
  private apiUrl = `${environment.apiUrl}/clientes`;

  constructor(private http: HttpClient) {}

  // Obtener todos los clientes
  getClientes(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(this.apiUrl);
  }

  // Obtener un cliente por ID
  getClienteById(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo cliente
  createCliente(clienteDto: ClienteDto): Observable<Cliente> {
    return this.http.post<Cliente>(this.apiUrl, clienteDto);
  }

  // Actualizar un cliente existente
  updateCliente(id: number, clienteDto: ClienteDto): Observable<Cliente> {
    return this.http.put<Cliente>(`${this.apiUrl}/${id}`, clienteDto);
  }

  // Eliminar un cliente
  deleteCliente(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
