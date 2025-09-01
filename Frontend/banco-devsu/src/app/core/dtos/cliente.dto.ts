import { CuentaDto } from "./cuenta.dto";

export class ClienteDto {
  constructor(
    public id?: number,
    public estado?: boolean,
    public nombre?: string,
    public identificacion?: string,
    public direccion?: string,
    public telefono?: string,
    public edad?: number,
    public genero?: string,
    public cuentas?: CuentaDto[],
    public contrasena?: string
  ) {}
}
