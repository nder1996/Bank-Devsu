import { Observable } from 'rxjs';
import { Cliente } from '../../models/Cliente';
import { ClienteDto } from '../../dtos/cliente.dto';


export interface IClienteService {
  getClientes(): Observable<Cliente[]>;
  getClienteById(id: number): Observable<Cliente>;
  createCliente(clienteDto: ClienteDto): Observable<Cliente>;
  updateCliente(id: number, clienteDto: ClienteDto): Observable<Cliente>;
  deleteCliente(id: number): Observable<void>;
}
