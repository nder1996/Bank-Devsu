import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  EstadoCuentaDto,
  ReporteEstadoCuentaResponseDto,
  ReporteEstadoCuentaRequestDto
} from 'src/app/core/dtos/reporte.dto';
import { ClienteDto } from 'src/app/core/dtos/cliente.dto';
import { ReporteService } from '../../../core/services/reporte.service';
import { ClienteService } from '../../../core/services/cliente.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.css']
})
export class ReportesComponent implements OnInit {
  // ===== DATA PROPERTIES =====
  clientes: ClienteDto[] = [];
  estadoCuentaData: EstadoCuentaDto[] = [];
  reporteResponse: ReporteEstadoCuentaResponseDto | null = null;

  // ===== PAGINATION =====
  paginatedData: EstadoCuentaDto[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  pages: number[] = [];

  // ===== UI STATE =====
  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  searchTerm = '';

  // ===== FORMS =====
  reporteForm!: FormGroup;
  formSubmitted = false;

  constructor(
    private reporteService: ReporteService,
    private clienteService: ClienteService,
    private fb: FormBuilder,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadClientes();
  }

  // ===== FORM SETUP =====
  private initForm(): void {
    this.reporteForm = this.fb.group({
      clienteId: ['', Validators.required],
      fechaInicio: ['', Validators.required],
      fechaFin: ['', Validators.required]
    });
  }

  // ===== DATA LOADING =====
  private loadClientes(): void {
    this.clienteService.getClientes().subscribe({
      next: (response: any) => {
        try {
          if (response && response.success) {
            this.clientes = response.data || [];
          } else {
            this.clientes = [];
            console.warn('Error loading clients:', response?.errors || 'Unknown error');
          }
        } catch (error) {
          console.error('Error processing clients response:', error);
          this.clientes = [];
        }
      },
      error: (error) => {
        console.error('Error loading clients:', error);
        this.clientes = [];
        this.showError('Error al cargar clientes');
      }
    });
  }

  // ===== GENERAR REPORTE =====
  generarReporte(): void {
    this.formSubmitted = true;

    Object.keys(this.reporteForm.controls).forEach(key => {
      this.reporteForm.get(key)?.markAsTouched();
    });

    if (this.reporteForm.invalid) {
      this.showError('Por favor complete todos los campos requeridos');
      return;
    }

    const formData = this.reporteForm.value;

    // Validar fechas
    const fechaInicio = new Date(formData.fechaInicio);
    const fechaFin = new Date(formData.fechaFin);

    if (fechaInicio > fechaFin) {
      this.showError('La fecha de inicio no puede ser mayor que la fecha de fin');
      return;
    }

    this.isLoading = true;
    this.clearMessages();

    this.reporteService.generarEstadoCuenta(
      Number(formData.clienteId),
      fechaInicio,
      fechaFin,
      'json'
    ).subscribe({
      next: (response: any) => {
        try {
          if (response && response.success) {
            this.reporteResponse = response.data;
            this.estadoCuentaData = response.data.datos || [];
            this.updatePagination();
            this.showSuccess(`Estado de cuenta generado con ${this.estadoCuentaData.length} registros`);
          } else {
            const errorMsg = response?.errors || response?.message || 'Error desconocido';
            this.showError('Error al generar estado de cuenta: ' + errorMsg);
          }
        } catch (error) {
          console.error('Error processing response:', error);
          this.showError('Error al procesar la respuesta del servidor');
        }
      },
      error: (error) => {
        console.error('Error generating report:', error);
        this.showError('Error al conectar con el servidor');
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  // ===== SEARCH & PAGINATION =====
  onSearch(): void {
    this.currentPage = 1;
    this.updatePagination();
  }

  private updatePagination(): void {
    let filteredData = [...this.estadoCuentaData];

    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase();
      filteredData = this.estadoCuentaData.filter(item =>
        item.cliente?.toLowerCase().includes(term) ||
        item.numeroCuenta?.toLowerCase().includes(term) ||
        item.tipo?.toLowerCase().includes(term)
      );
    }

    this.totalPages = Math.ceil(filteredData.length / this.pageSize);
    this.generatePageNumbers();

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedData = filteredData.slice(startIndex, endIndex);
  }

  private generatePageNumbers(): void {
    this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  // ===== DOWNLOAD OPERATIONS =====
  downloadPdf(): void {
    if (!this.reporteForm.valid) {
      this.showError('Debe generar un reporte primero');
      return;
    }

    const formData = this.reporteForm.value;
    const fechaInicio = new Date(formData.fechaInicio);
    const fechaFin = new Date(formData.fechaFin);

    this.isLoading = true;

    this.reporteService.descargarEstadoCuentaPdf(
      Number(formData.clienteId),
      fechaInicio,
      fechaFin
    ).subscribe({
      next: (blob: Blob) => {
        const fileName = `estado_cuenta_${formData.clienteId}_${formData.fechaInicio}_${formData.fechaFin}.pdf`;
        this.downloadBlob(blob, fileName);
        this.showSuccess('Reporte PDF descargado exitosamente');
      },
      error: (error) => {
        console.error('Error downloading PDF:', error);
        this.showError('Error al descargar reporte PDF');
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  downloadJson(): void {
    if (!this.reporteForm.valid) {
      this.showError('Debe generar un reporte primero');
      return;
    }

    const formData = this.reporteForm.value;
    const fechaInicio = new Date(formData.fechaInicio);
    const fechaFin = new Date(formData.fechaFin);

    this.isLoading = true;

    this.reporteService.descargarEstadoCuentaJson(
      Number(formData.clienteId),
      fechaInicio,
      fechaFin
    ).subscribe({
      next: (blob: Blob) => {
        const fileName = `estado_cuenta_${formData.clienteId}_${formData.fechaInicio}_${formData.fechaFin}.json`;
        this.downloadBlob(blob, fileName);
        this.showSuccess('Reporte JSON descargado exitosamente');
      },
      error: (error) => {
        console.error('Error downloading JSON:', error);
        this.showError('Error al descargar reporte JSON');
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  private downloadBlob(blob: Blob, fileName: string): void {
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    link.click();
    window.URL.revokeObjectURL(url);
  }

  // ===== MANEJO DE MENSAJES =====
  private showError(message: string): void {
    this.notificationService.showError(message);
  }

  private showSuccess(message: string): void {
    this.notificationService.showSuccess(message);
  }

  private clearMessages(): void {
    this.errorMessage = null;
    this.successMessage = null;
  }

  private autoHideMessage(): void {
    setTimeout(() => {
      this.clearMessages();
    }, 5000);
  }

  // ===== UTILITY GETTERS =====
  get startIndex(): number {
    return this.estadoCuentaData.length > 0 ? (this.currentPage - 1) * this.pageSize + 1 : 0;
  }

  get endIndex(): number {
    return Math.min(this.currentPage * this.pageSize, this.estadoCuentaData.length);
  }

  get totalItems(): number {
    return this.estadoCuentaData.length;
  }

  get hasData(): boolean {
    return this.paginatedData.length > 0;
  }

  get showNoData(): boolean {
    return !this.isLoading && !this.hasData;
  }

  getClienteNombre(clienteId: number): string {
    const cliente = this.clientes.find(c => c.id === clienteId);
    return cliente?.nombre || 'Cliente no encontrado';
  }
}
