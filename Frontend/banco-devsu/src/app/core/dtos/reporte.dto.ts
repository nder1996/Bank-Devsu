// ===== NUEVO DTO SIMPLIFICADO PARA ESTADO DE CUENTA =====

export interface EstadoCuentaDto {
  fecha: string;
  cliente: string;
  numeroCuenta: string;
  tipo: string;
  saldoInicial: number;
  estado: boolean;
  movimiento: number;
  saldoDisponible: number;
}

export interface ReporteEstadoCuentaResponseDto {
  datos: EstadoCuentaDto[];
  pdfBase64?: string;
  totalDebitos: number;
  totalCreditos: number;
  totalMovimientos: number;
  fechaGeneracion: Date;
}

export interface ReporteEstadoCuentaRequestDto {
  clienteId: number;
  fechaInicio: Date;
  fechaFin: Date;
  formato?: string;
}