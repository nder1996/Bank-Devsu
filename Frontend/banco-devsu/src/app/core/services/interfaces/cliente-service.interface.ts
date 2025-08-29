import { Observable } from 'rxjs';
import { ClienteDto } from '../../dtos/cliente.dto';


export interface IClienteService {
  getClientes(): Observable<ClienteDto[]>;
  getClienteById(id: number): Observable<ClienteDto>;
  createCliente(clienteDto: ClienteDto): Observable<ClienteDto>;
  updateCliente(id: number, clienteDto: ClienteDto): Observable<ClienteDto>;
  deleteCliente(id: number): Observable<void>;
}
