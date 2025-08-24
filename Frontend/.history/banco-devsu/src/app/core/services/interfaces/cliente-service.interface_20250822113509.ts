import { Observable } from 'rxjs';
import { Cliente } from '../../models/Cliente';
import { ClienteDTO } from '../../dtos/cliente.dto';

export interface IClienteService {
  getClientes(): Observable<Cliente[]>;
  getClienteById(id: number): Observable<Cliente>;
  createCliente(clienteDto: ClienteDTO): Observable<Cliente>;
  updateCliente(id: number, clienteDto: ClienteDTO): Observable<Cliente>;
  deleteCliente(id: number): Observable<void>;
}
