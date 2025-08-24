import { TipoMovimiento } from '../enums/tipo-movimiento.enum';

export class CuentaResumida {
  constructor(
    public id?: number,
    public numeroCuenta?: string
  ) {}
}

export class MovimientoDto {
  constructor(
    public id?: number,
    public fecha?: Date,
    public tipoMovimiento?: TipoMovimiento,
    public valor?: number,
    public saldo?: number,
    public cuenta?: CuentaResumida
  ) {}
}
