export interface ClienteDTO {
    id: number;
    contrasena: string;
    estado: boolean;
    persona: PersonaDTO; // Relación con Persona
}
