import { PersonaDto } from "./persona.dto";
import { CuentaDto } from "./cuenta.dto";

export class ClienteDto extends PersonaDto {
  constructor(
    id: number,
    nombre: string,
    genero: string,
    edad: number,
    identificacion: string,
    direccion: string,
    telefono: string,
    public clienteId: number,
    public contrasena: string,
    public estado: boolean,
    public cuentas?: CuentaDto[]
  ) {
    super(id, nombre, genero, edad, identificacion, direccion, telefono);
  }
}
