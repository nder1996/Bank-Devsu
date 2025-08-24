import { TipoMovimiento } from "../enums/tipo-movimiento.enum";
import { Cuenta } from "./Cuenta";

export interface Movimiento {
  id?: number | null;
  fecha?: Date | null;
  tipoMovimiento?: TipoMovimiento | TipoMovimiento;  // o enum si lo tienes definido
  valor?: number | null;
  saldo?: number | null;
  cuentaId?: number | null;
  cuenta?: Cuenta | null;
}


