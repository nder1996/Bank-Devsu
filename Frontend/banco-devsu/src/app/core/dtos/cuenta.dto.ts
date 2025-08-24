import { TipoCuenta } from '../enums/tipo-cuenta.enum';

export class ClienteResumido {
  constructor(
    public id?: number,
    public nombre?: string
  ) {}
}

export class CuentaDto {
  constructor(
    public id?: number,
    public numeroCuenta?: string,
    public tipoCuenta?: TipoCuenta,
    public saldoInicial?: number,
    public estado?: boolean,
    public cliente?: ClienteResumido
  ) {}
}
