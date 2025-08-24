import { Persona } from "./Persona";
import { Cuenta } from "./Cuenta";

export interface Cliente {
  id?: number | null;
  contrasena?: string | null;
  estado?: boolean | null;
  personaId?: number | null;
  persona?: Persona | null;
  cuentas?: Cuenta[] | null;
}
