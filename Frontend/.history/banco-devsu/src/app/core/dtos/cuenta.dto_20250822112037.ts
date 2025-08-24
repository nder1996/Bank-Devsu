import { TipoCuenta } from '../enums/tipo-cuenta.enum';
import { ClienteDTO } from './cliente.dto';

export interface CuentaDTO {
    id: number;
    numeroCuenta: string;
    tipoCuenta: TipoCuenta;
    saldoInicial: number;
    estado: boolean;
    cliente: ClienteDTO;
}
