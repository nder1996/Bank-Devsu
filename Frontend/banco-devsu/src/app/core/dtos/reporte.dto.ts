export class ReporteDto {
  public id: number = 0;
  public clienteId: number = 0;
  public fechaInicio: Date = new Date();
  public fechaFin: Date = new Date();
  public formato: ReporteFormato = ReporteFormato.JSON;
  public fechaGeneracion: Date = new Date();
  public rutaArchivo: string | null = null;
  public nombreArchivo: string | null = null;
  public activo: boolean = true;
  public cliente: ReporteDto.ClienteResumido = new ReporteDto.ClienteResumido();
}

export namespace ReporteDto {
  export class ClienteResumido {
    public id: number = 0;
    public nombre: string = '';
  }
}

export enum ReporteFormato {
  JSON = 0,
  PDF = 1
}

export class ReporteRequestDto {
  public ClienteId: number = 0;
  public FechaInicio: Date = new Date();
  public FechaFin: Date = new Date();
  public Formato: ReporteFormato = ReporteFormato.JSON;
}