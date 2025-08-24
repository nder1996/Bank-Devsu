import { TipoCuenta } from '../enums/tipo-cuenta.enum';
import { ClienteDTO } from './cliente.dto';

export interface CuentaDTO {
  cuentaId: number;
  tipo: number; // 1 para Crédito, 0 para Débito
  valor: number;
  saldo: number;
  fecha: string; // Fecha en formato ISO
}
