import { TipoMovimiento } from "../enums/tipo-movimiento.enum";
import { Cuenta } from "./Cuenta";

export interface Movimiento {
  Id?: number | null;
  Fecha?: Date | null;
  TipoMovimiento?: TipoMovimiento;  // Cambiado a TipoMovimiento
  Valor?: number | null;
  Saldo?: number | null;
  cuentaId?: number | null;
  cuenta?: Cuenta | null;
}


