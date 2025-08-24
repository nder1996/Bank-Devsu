import { TipoMovimiento } from '../enums/tipo-movimiento.enum';
import { CuentaDTO } from './cuenta.dto';

export interface MovimientoDTO {
    id: number;
    fecha: Date;
    tipoMovimiento: TipoMovimiento;
    valor: number;
    Saldo: number;
    CuentaId: number;
}
