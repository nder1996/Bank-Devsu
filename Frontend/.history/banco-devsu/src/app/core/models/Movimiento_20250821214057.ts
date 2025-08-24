import { Cuenta } from "./Cuenta";
import { Cliente } from "./Cliente";
import { Persona } from "./Persona";

export interface Movimiento {
  id?: number | null;
  fecha?: Date | null;
  tipoMovimiento?: string | null;  // o enum si lo tienes definido
  valor?: number | null;
  saldo?: number | null;
  cuentaId?: number | null;
  cuenta?: Cuenta | null;
}

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

export interface Cliente {
  id?: number | null;
  contrasena?: string | null;
  estado?: boolean | null;
  personaId?: number | null;
  persona?: Persona | null;
  cuentas?: Cuenta[] | null;
}

export interface Persona {
  id?: number | null;
  nombre?: string | null;
  genero?: string | null;
  edad?: number | null;
  identificacion?: string | null;
  direccion?: string | null;
  telefono?: string | null;
}


