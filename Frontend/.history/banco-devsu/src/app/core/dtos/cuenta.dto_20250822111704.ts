import { ClienteDTO } from './cliente.dto';

export interface CuentaDTO {
    id: number;
    numeroCuenta: string;
    tipoCuenta: string;
    saldoInicial: number;
    estado: boolean;
    cliente: ClienteDTO;
}
