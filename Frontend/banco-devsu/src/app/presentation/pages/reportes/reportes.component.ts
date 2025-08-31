import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReporteDto, ReporteRequestDto, ReporteFormato } from 'src/app/core/dtos/reporte.dto';
import { ClienteDto } from 'src/app/core/dtos/cliente.dto';
import { ReporteService } from '../../../core/services/reporte.service';
import { ClienteService } from '../../../core/services/cliente.service';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.css']
})
export class ReportesComponent implements OnInit {
  // Data properties
  reportes: ReporteDto[] = [];
  filteredReportes: ReporteDto[] = [];
  clientes: ClienteDto[] = [];
  
  // Pagination
  paginatedReportes: ReporteDto[] = [];
  currentPage = 1;
  pageSize = 5;
  totalPages = 0;
  pages: number[] = [];

  // UI State
  isLoading = false;
  errorMessage: string | null = null;
  searchTerm = '';

  // Modal states
  isModalVisible = false;
  isDeleteModalVisible = false;
  isEditMode = false;
  reporteToDelete: ReporteDto | null = null;

  // Form
  reporteForm!: FormGroup;
  formSubmitted = false;

  // Enums for template
  ReporteFormato = ReporteFormato;

  constructor(
    private reporteService: ReporteService,
    private clienteService: ClienteService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadClientes();
    this.loadReportes();
  }

  // === FORM SETUP ===
  private initForm(): void {
    this.reporteForm = this.fb.group({
      id: [null],
      clienteId: ['', Validators.required],
      fechaInicio: ['', Validators.required],
      fechaFin: ['', Validators.required],
      formato: [ReporteFormato.JSON, Validators.required],
      activo: [true, Validators.required]
    });
  }

  // === DATA LOADING ===
  private loadClientes(): void {
    this.clienteService.getClientes().subscribe({
      next: (response: any) => {
        if (response.success) {
          this.clientes = response.data;
        }
      },
      error: (error) => {
        console.error('Error loading clients:', error);
      }
    });
  }

  private loadReportes(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.reporteService.getReportes().subscribe({
      next: (response: any) => {
        if (response.success) {
          this.reportes = this.mapReportesFromResponse(response.data);
          this.applyFilter();
        } else {
          this.errorMessage = 'Error al cargar reportes: ' + (response.errors || 'Error desconocido');
        }
      },
      error: () => {
        this.errorMessage = 'Error al conectar con el servidor';
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  private mapReportesFromResponse(data: ReporteDto[]): ReporteDto[] {
    return data.map(reporte => ({
      id: reporte.id,
      clienteId: reporte.clienteId,
      fechaInicio: new Date(reporte.fechaInicio),
      fechaFin: new Date(reporte.fechaFin),
      formato: reporte.formato,
      fechaGeneracion: new Date(reporte.fechaGeneracion),
      rutaArchivo: reporte.rutaArchivo,
      nombreArchivo: reporte.nombreArchivo,
      activo: reporte.activo,
      cliente: reporte.cliente
    }));
  }

  // === SEARCH & PAGINATION ===
  onSearch(): void {
    this.currentPage = 1;
    this.applyFilter();
  }

  private applyFilter(): void {
    if (!this.searchTerm.trim()) {
      this.filteredReportes = [...this.reportes];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredReportes = this.reportes.filter(reporte =>
        this.searchInReporte(reporte, term)
      );
    }
    this.updatePagination();
  }

  private searchInReporte(reporte: ReporteDto, term: string): boolean {
    return (
      (reporte.cliente.nombre?.toLowerCase().includes(term) ?? false) ||
      (reporte.nombreArchivo?.toLowerCase().includes(term) ?? false) ||
      (ReporteFormato[reporte.formato]?.toLowerCase().includes(term) ?? false) ||
      (reporte.fechaGeneracion.toLocaleDateString().includes(term) ?? false)
    );
  }

  private updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredReportes.length / this.pageSize);
    this.generatePageNumbers();
    this.updateCurrentPage();
  }

  private generatePageNumbers(): void {
    this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  private updateCurrentPage(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedReportes = this.filteredReportes.slice(startIndex, endIndex);
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updateCurrentPage();
    }
  }

  // === MODAL MANAGEMENT ===
  openCreateModal(): void {
    this.resetModal();
    this.isEditMode = false;
    this.isModalVisible = true;
  }

  openEditModal(reporte: ReporteDto): void {
    this.resetModal();
    this.isEditMode = true;
    
    this.reporteForm.patchValue({
      id: reporte.id,
      clienteId: reporte.clienteId,
      fechaInicio: this.formatDateForInput(reporte.fechaInicio),
      fechaFin: this.formatDateForInput(reporte.fechaFin),
      formato: reporte.formato,
      activo: reporte.activo
    });
    
    this.isModalVisible = true;
  }

  closeModal(): void {
    this.isModalVisible = false;
    this.resetModal();
  }

  private resetModal(): void {
    this.reporteForm.reset();
    this.reporteForm.patchValue({ 
      formato: ReporteFormato.JSON,
      activo: true 
    });
    this.formSubmitted = false;
  }

  private formatDateForInput(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  // === CRUD OPERATIONS ===
  saveReporte(): void {
    this.formSubmitted = true;
    
    Object.keys(this.reporteForm.controls).forEach(key => {
      this.reporteForm.get(key)?.markAsTouched();
    });

    if (this.reporteForm.invalid) {
      return;
    }

    const formData = this.reporteForm.value;

    if (this.isEditMode) {
      const reporteData: ReporteDto = {
        id: formData.id,
        clienteId: Number(formData.clienteId),
        fechaInicio: new Date(formData.fechaInicio),
        fechaFin: new Date(formData.fechaFin),
        formato: Number(formData.formato),
        fechaGeneracion: new Date(),
        rutaArchivo: null,
        nombreArchivo: null,
        activo: formData.activo,
        cliente: { id: 0, nombre: '' }
      };

      this.reporteService.updateReporte(reporteData.id, reporteData).subscribe({
        next: () => {
          this.closeModal();
          this.loadReportes();
        },
        error: (error) => {
          console.error('Error updating report:', error);
          this.errorMessage = 'Error al actualizar reporte';
        }
      });
    } else {
      const reporteRequestData: ReporteRequestDto = {
        ClienteId: Number(formData.clienteId),
        FechaInicio: new Date(formData.fechaInicio),
        FechaFin: new Date(formData.fechaFin),
        Formato: Number(formData.formato)
      };

      this.reporteService.createReporte(reporteRequestData).subscribe({
        next: () => {
          this.closeModal();
          this.loadReportes();
        },
        error: (error) => {
          console.error('Error creating report:', error);
          this.errorMessage = 'Error al crear reporte';
        }
      });
    }
  }

  // === DELETE OPERATIONS ===
  openDeleteModal(reporte: ReporteDto): void {
    this.reporteToDelete = reporte;
    this.isDeleteModalVisible = true;
  }

  closeDeleteModal(): void {
    this.reporteToDelete = null;
    this.isDeleteModalVisible = false;
  }

  confirmDelete(): void {
    if (!this.reporteToDelete) return;

    this.reporteService.deleteReporte(this.reporteToDelete.id!).subscribe({
      next: () => {
        this.closeDeleteModal();
        this.loadReportes();
      },
      error: (error) => {
        console.error('Error deleting report:', error);
        this.errorMessage = 'Error al eliminar reporte';
        this.closeDeleteModal();
      }
    });
  }

  // === DOWNLOAD OPERATIONS ===
  downloadReporte(reporte: ReporteDto): void {
    this.reporteService.downloadReporte(reporte.id).subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = reporte.nombreArchivo || 'reporte';
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (error) => {
        console.error('Error downloading report:', error);
        this.errorMessage = 'Error al descargar reporte';
      }
    });
  }

  // === UTILITY GETTERS ===
  get startIndex(): number {
    return this.filteredReportes.length > 0 ? (this.currentPage - 1) * this.pageSize + 1 : 0;
  }

  get endIndex(): number {
    return Math.min(this.currentPage * this.pageSize, this.filteredReportes.length);
  }

  get hasData(): boolean {
    return this.paginatedReportes.length > 0;
  }

  get showNoData(): boolean {
    return !this.isLoading && !this.hasData;
  }

  getFormatoLabel(formato: ReporteFormato): string {
    return ReporteFormato[formato];
  }
}
