import { TipoMovimiento } from "../enums/tipo-movimiento.enum";
import { Cuenta } from "./Cuenta";

export interface Movimiento {
  Id?: number | null;
  fecha?: Date | null;
  tipoMovimiento?: TipoMovimiento;  // Cambiado a TipoMovimiento
  valor?: number | null;
  saldo?: number | null;
  cuentaId?: number | null;
  cuenta?: Cuenta | null;
}


