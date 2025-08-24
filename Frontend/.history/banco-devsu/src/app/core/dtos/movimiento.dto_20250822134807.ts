import { TipoMovimiento } from '../enums/tipo-movimiento.enum';

export interface MovimientoDTO {
  id: number;

  // La fecha es requerida
  fecha: string; // Usamos string para manejar formato ISO o local

  // El tipo de movimiento es requerido
  tipoMovimiento: 'Debito' | 'Credito'; // "Debito" o "Credito"

  // El valor es requerido
  valor: number;

  // El saldo es requerido
  saldo: number;

  // La cuenta es requerida
  cuentaId: number;
}
