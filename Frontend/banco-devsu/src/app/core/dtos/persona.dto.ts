export class PersonaDto {
  constructor(
    public id: number,
    public nombre: string,
    public genero: string,
    public edad: number,
    public identificacion: string,
    public direccion: string,
    public telefono: string
  ) {}
}