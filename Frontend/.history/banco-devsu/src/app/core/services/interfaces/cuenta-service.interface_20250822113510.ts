import { Observable } from 'rxjs';
import { Cuenta } from '../../models/Cuenta';
import { CuentaDTO } from '../../dtos/cuenta.dto';

export interface ICuentaService {
  getCuentas(): Observable<Cuenta[]>;
  getCuentaById(id: number): Observable<Cuenta>;
  createCuenta(cuentaDto: CuentaDTO): Observable<Cuenta>;
  updateCuenta(id: number, cuentaDto: CuentaDTO): Observable<Cuenta>;
  deleteCuenta(id: number): Observable<void>;
}
