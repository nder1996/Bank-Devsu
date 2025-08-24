import { ClienteDTO } from './cliente.dto';

export interface CuentaDTO {
    id: number;
    numeroCuenta: string;
    tipoCuenta: string; // Puede ser un enum
    saldoInicial: number;
    estado: boolean;
    cliente: ClienteDTO; // Relación con Cliente
}
