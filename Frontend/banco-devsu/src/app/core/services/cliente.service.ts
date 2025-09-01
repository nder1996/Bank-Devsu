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
  private apiUrl = `${environment.apiUrl}/Clientes`;

  constructor(private http: HttpClient) {}

  // Obtener todos los clientes
  getClientes(): Observable<ClienteDto[]> {
    return this.http.get<ClienteDto[]>(this.apiUrl);
  }

  // Obtener un cliente por ID
  getClienteById(id: number): Observable<ClienteDto> {
    return this.http.get<ClienteDto>(`${this.apiUrl}/${id}`);
  }

  // Crear un nuevo cliente
  createCliente(clienteDto: ClienteDto): Observable<any> {
    console.log('Datos a enviar:', JSON.stringify(clienteDto) );
    return this.http.post<any>(this.apiUrl, clienteDto);
  }

  // Actualizar un cliente existente
  updateCliente(id: number, clienteDto: ClienteDto): Observable<any> {
    console.log('Datos de actualización a enviar:', JSON.stringify(clienteDto));
    return this.http.put<any>(`${this.apiUrl}/${id}`, clienteDto);
  }

  // Eliminar un cliente
  deleteCliente(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
