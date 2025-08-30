import { TipoMovimiento } from '../enums/tipo-movimiento.enum';

export class MovimientoDto {
  public id: number = 0;
  public fecha: Date = new Date();
  public tipoMovimiento: TipoMovimiento = TipoMovimiento.Credito;
  public valor: number = 0;
  public saldo: number = 0;
  public cuenta: MovimientoDto.CuentaResumida = new MovimientoDto.CuentaResumida();
}

export namespace MovimientoDto {
  export class CuentaResumida {
    public id: number = 0;
    public numeroCuenta: string = '';
  }
}
