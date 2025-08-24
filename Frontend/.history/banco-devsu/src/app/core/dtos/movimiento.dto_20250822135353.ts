import { TipoMovimiento } from '../enums/tipo-movimiento.enum';
import { CuentaDTO } from './cuenta.dto';

export interface MovimientoDTO {
    id: number;
    Fecha: Date;
    TipoMovimiento: TipoMovimiento;
    Valor: number;
    Saldo: number;
    CuentaId: number;
}
