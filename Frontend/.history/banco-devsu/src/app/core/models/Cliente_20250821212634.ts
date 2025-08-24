import { Persona } from "./Persona";

export interface Cliente {
  id?: number | null;
  contrasena?: string | null;
  estado?: boolean | null;
  personaId?: number | null;
  persona?: Persona | null;
  cuentas?: any[] | null; // o importa el modelo Cuenta si lo tienes
}
