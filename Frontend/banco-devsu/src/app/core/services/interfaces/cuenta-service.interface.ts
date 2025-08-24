import { Observable } from 'rxjs';
import { Cuenta } from '../../models/Cuenta';
import { CuentaDto } from '../../dtos/cuenta.dto';


export interface ICuentaService {
  getCuentas(): Observable<Cuenta[]>;
  getCuentaById(id: number): Observable<Cuenta>;
  createCuenta(cuentaDto: CuentaDto): Observable<Cuenta>;
  updateCuenta(id: number, cuentaDto: CuentaDto): Observable<Cuenta>;
  deleteCuenta(id: number): Observable<void>;
}
