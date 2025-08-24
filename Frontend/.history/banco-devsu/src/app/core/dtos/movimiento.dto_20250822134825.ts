import { TipoMovimiento } from '../enums/tipo-movimiento.enum';
import { CuentaDTO } from './cuenta.dto';

export interface MovimientoDTO {
    Id: number;
    Fecha: Date;
    tipoMovimiento: TipoMovimiento;
    valor: number;
    saldo: number;
    cuenta: CuentaDTO;
}
