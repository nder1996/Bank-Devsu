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
  createCliente(clienteData: any): Observable<any> {
    // Simplificar estructura para coincidir con el DTO del backend
    const clienteDto = {
      id: clienteData.id || 0,
      estado: clienteData.estado !== undefined ? clienteData.estado : true,
      nombre: clienteData.persona?.nombre || clienteData.nombre,
      identificacion: clienteData.persona?.identificacion || clienteData.identificacion,
      direccion: clienteData.persona?.direccion || clienteData.direccion,
      telefono: clienteData.persona?.telefono || clienteData.telefono,
      edad: clienteData.persona?.edad || clienteData.edad,
      genero: clienteData.persona?.genero || clienteData.genero,
      cuentas: clienteData.cuentas || []
    };

    console.log('Datos a enviar:', clienteDto);
    return this.http.post<any>(this.apiUrl, clienteDto);
  }

  // Actualizar un cliente existente
  updateCliente(id: number, clienteData: any): Observable<any> {
    // Simplificar estructura para coincidir con el DTO del backend
    const clienteDto = {
      id: clienteData.id || id,
      estado: clienteData.estado !== undefined ? clienteData.estado : true,
      nombre: clienteData.persona?.nombre || clienteData.nombre,
      identificacion: clienteData.persona?.identificacion || clienteData.identificacion,
      direccion: clienteData.persona?.direccion || clienteData.direccion,
      telefono: clienteData.persona?.telefono || clienteData.telefono,
      edad: clienteData.persona?.edad || clienteData.edad,
      genero: clienteData.persona?.genero || clienteData.genero,
      cuentas: clienteData.cuentas || []
    };

    console.log('Datos de actualización a enviar:', JSON.stringify(clienteDto));
    return this.http.put<any>(`${this.apiUrl}/${id}`, clienteDto);
  }

  // Eliminar un cliente
  deleteCliente(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
