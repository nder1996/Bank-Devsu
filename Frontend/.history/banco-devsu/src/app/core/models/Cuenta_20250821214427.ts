import { Cliente } from "./Cliente";
import { Movimiento } from "./Movimiento";

export interface Cuenta {
  id?: number | null;
  numeroCuenta?: string | null;
  tipoCuenta?: string | null;  // o enum si lo tienes definido
  saldoInicial?: number | null;
  estado?: boolean | null;
  clienteId?: number | null;
  clienteNavigation?: Cliente | null;
  movimientos?: Movimiento[] | null;
}
