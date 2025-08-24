import { PersonaDTO } from "./persona.dto";

export interface ClienteDTO {
    id: number;
    contrasena: string;
    estado: boolean;
    persona: PersonaDTO; 
}
