import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MovimientoService } from 'src/app/core/services/movimiento.service';
import { CuentaService } from 'src/app/core/services/cuenta.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { TipoMovimiento } from '../../../core/enums/tipo-movimiento.enum';
import { MovimientoDto } from 'src/app/core/dtos/movimiento.dto';
import { CuentaDto } from 'src/app/core/dtos/cuenta.dto';


@Component({
  selector: 'app-movimientos',
  templateUrl: './movimientos.component.html',
  styleUrls: ['./movimientos.component.css']
})
export class MovimientosComponent implements OnInit {
  // Propiedades del formulario
  movimientoForm!: FormGroup;
  formSubmitted = false;
  isEditMode = false;
  editingMovimientoId: number | null = null;

  // Propiedades de datos
  movimientos: MovimientoDto[] = [];
  cuentas: CuentaDto[] = [];
  filteredItems: MovimientoDto[] = [];
  paginatedItems: MovimientoDto[] = [];

  // Variables de búsqueda y paginación
  searchTerm: string = '';
  currentPage = 1;
  pageSize = 5;
  totalPages = 0;
  pages: number[] = [];
  startIndex = 0;
  endIndex = 0;

  // Modal states
  isModalVisible = false;
  isDeleteModalVisible = false;
  movimientoToDelete: MovimientoDto | null = null;

  // Enums para el template
  TipoMovimiento = TipoMovimiento;

  constructor(
    private movimientoService: MovimientoService,
    private cuentaService: CuentaService,
    private fb: FormBuilder,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadData();
  }

  private initForm(): void {
    this.movimientoForm = this.fb.group({
      cuentaId: ['', [Validators.required]],
      tipoMovimiento: ['', [Validators.required]],
      valor: ['', [Validators.required, Validators.min(0.01)]]
    });
  }

  private loadData(): void {
    // Cargar cuentas primero, luego movimientos para filtrar correctamente
    this.cargarCuentas();
  }

  private cargarCuentas(): void {
    this.cuentaService.getCuentas().subscribe({
      next: (response: any) => {
        if (response.success && response.data) {
          this.cuentas = response.data;
          // Cargar movimientos después de tener las cuentas cargadas
          this.cargarMovimientos();
        } else {
          console.error('Error en la respuesta del servicio al cargar cuentas:', response);
          this.notificationService.showError('Error al cargar cuentas');
        }
      },
      error: (err) => {
        console.error('Error en la suscripción al cargar cuentas:', err);
        this.notificationService.showError('Error al conectar con el servidor');
      }
    });
  }

  private cargarMovimientos(): void {
    this.movimientoService.getMovimientos().subscribe({
      next: (response: any) => {
        if (response.success && response.data) {
          // Crear array de IDs de cuentas activas
          const idsDesCuentasActivas = this.cuentas
            .filter(cuenta => cuenta.estado === true)
            .map(cuenta => cuenta.id);

          // Filtrar movimientos solo de cuentas activas
          const movimientosFiltrados = response.data.filter((movimiento: MovimientoDto) => {
            return movimiento.cuenta && idsDesCuentasActivas.includes(movimiento.cuenta.id);
          });

          this.movimientos = movimientosFiltrados;
          this.filteredItems = [...this.movimientos];
          this.updatePagination();
          this.updatePage();
        } else {
          this.notificationService.showError('Error al cargar movimientos');
        }
      },
      error: () => {
        this.notificationService.showError('Error al conectar con el servidor');
      }
    });
  }

  // CRUD Operations
  saveMovimiento(): void {
    this.formSubmitted = true;

    if (this.movimientoForm.invalid) {
      this.notificationService.showWarning('Complete todos los campos requeridos');
      this.markFormGroupTouched(this.movimientoForm);
      return;
    }

    const movimientoDto = this.createMovimientoDto();

    if (this.isEditMode && this.editingMovimientoId) {
      this.updateMovimiento(this.editingMovimientoId, movimientoDto);
    } else {
      this.createMovimiento(movimientoDto);
    }
  }

  private createMovimiento(movimientoDto: MovimientoDto): void {
    this.movimientoService.createMovimiento(movimientoDto).subscribe({
      next: (response: any) => {
        if (response.success) {
          this.notificationService.showSuccess('Movimiento creado exitosamente');
          this.closeModal();
          this.cargarMovimientos();
        } else {
          this.notificationService.showError('Error al crear el movimiento');
        }
      },
      error: () => {
        this.notificationService.showError('Error al crear el movimiento');
      }
    });
  }

  private updateMovimiento(id: number, movimientoDto: MovimientoDto): void {
    this.movimientoService.updateMovimiento(id, movimientoDto).subscribe({
      next: (response: any) => {
        if (response.success) {
          this.notificationService.showSuccess('Movimiento actualizado exitosamente');
          this.closeModal();
          this.cargarMovimientos();
        } else {
          this.notificationService.showError('Error al actualizar el movimiento');
        }
      },
      error: () => {
        this.notificationService.showError('Error al actualizar el movimiento');
      }
    });
  }

  private deleteMovimiento(): void {
    if (!this.movimientoToDelete?.id) return;

    this.movimientoService.deleteMovimiento(this.movimientoToDelete.id).subscribe({
      next: (response: any) => {
        if (response.success) {
          this.notificationService.showSuccess('Movimiento eliminado exitosamente');
          this.closeDeleteModal();
          this.cargarMovimientos();
        } else {
          this.notificationService.showError('Error al eliminar el movimiento');
        }
      },
      error: () => {
        this.notificationService.showError('Error al eliminar el movimiento');
      }
    });
  }

  // Modal Methods
  openCreateModal(): void {
    this.isModalVisible = true;
    this.isEditMode = false;
    this.editingMovimientoId = null;
    this.formSubmitted = false;
    this.movimientoForm.reset();
    this.movimientoForm.get('cuentaId')?.enable();
  }

  openEditModal(movimiento: MovimientoDto): void {
    if (movimiento?.id && movimiento.cuenta) {
      this.cargarCuentas()
      this.isModalVisible = true;
      this.isEditMode = true;
      this.editingMovimientoId = movimiento.id;
      this.formSubmitted = false;

      this.movimientoForm.patchValue({
        cuentaId: movimiento.cuenta.id,
        tipoMovimiento: movimiento.tipoMovimiento,
        valor: Math.abs(movimiento.valor)
      });

      // Deshabilitar el campo cuentaId para modo edición
      this.movimientoForm.get('cuentaId')?.disable();
    }
  }

  closeModal(): void {
    this.isModalVisible = false;
    this.isEditMode = false;
    this.editingMovimientoId = null;
    this.formSubmitted = false;
    this.movimientoForm.reset();

    // Habilitar el campo cuentaId al cerrar el modal
    this.movimientoForm.get('cuentaId')?.enable();
  }

  openDeleteModal(movimiento: MovimientoDto): void {
    this.movimientoToDelete = movimiento;
    this.isDeleteModalVisible = true;
  }

  closeDeleteModal(): void {
    this.isDeleteModalVisible = false;
    this.movimientoToDelete = null;
  }

  confirmDelete(): void {
    this.deleteMovimiento();
  }

  // Helper Methods
  private createMovimientoDto(): MovimientoDto {
    // Usar getRawValue() para obtener valores de campos deshabilitados también
    const formValue = this.movimientoForm.getRawValue();
    const cuentaId = parseInt(formValue.cuentaId);
    const selectedCuenta = this.cuentas.find(c => c.id === cuentaId);

    if (!selectedCuenta) {
      throw new Error(`No se encontró la cuenta con ID: ${cuentaId}`);
    }

    const movimientoDto = new MovimientoDto();
    movimientoDto.id = this.isEditMode ? this.editingMovimientoId! : 0;
    movimientoDto.fecha = new Date();
    movimientoDto.tipoMovimiento = parseInt(formValue.tipoMovimiento) as TipoMovimiento;
    movimientoDto.valor = parseFloat(formValue.valor);
    movimientoDto.saldo = 0;
    movimientoDto.cuenta = new MovimientoDto.CuentaResumida();
    movimientoDto.cuenta.id = selectedCuenta.id!;
    movimientoDto.cuenta.numeroCuenta = selectedCuenta.numeroCuenta!;

    return movimientoDto;
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  // Search and Pagination
  filterItems(): void {
    if (!this.searchTerm.trim()) {
      this.filteredItems = [...this.movimientos];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredItems = this.movimientos.filter(item =>
        (item.cuenta?.numeroCuenta || '').toLowerCase().includes(term) ||
        (item.tipoMovimiento === TipoMovimiento.Credito ? 'crédito' : 'débito').includes(term) ||
        (item.valor || 0).toString().includes(term) ||
        (item.saldo || 0).toString().includes(term)
      );
    }
    this.currentPage = 1;
    this.updatePagination();
    this.updatePage();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredItems.length / this.pageSize);
    this.generatePages();
  }

  generatePages(): void {
    this.pages = [];
    for (let i = 1; i <= this.totalPages; i++) {
      this.pages.push(i);
    }
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePage();
    }
  }

  updatePage(): void {
    this.startIndex = (this.currentPage - 1) * this.pageSize;
    this.endIndex = Math.min(this.startIndex + this.pageSize, this.filteredItems.length);
    this.paginatedItems = this.filteredItems.slice(this.startIndex, this.endIndex);
  }
}
