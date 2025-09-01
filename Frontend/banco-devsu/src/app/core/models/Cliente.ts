import { Persona } from "./Persona";
import { Cuenta } from "./Cuenta";

export interface Cliente extends Persona {
  clienteId: number;
  contrasena: string;
  estado: boolean;
  cuentas?: Cuenta[] | null;
}
