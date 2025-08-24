import { CuentaDTO } from './cuenta.dto';

export interface MovimientoDTO {
    id: number;
    fecha: Date;
    tipoMovimiento: TipoMovimiento; // Puede ser un enum
    valor: number;
    saldo: number;
    cuenta: CuentaDTO; // Relación con Cuenta
}
