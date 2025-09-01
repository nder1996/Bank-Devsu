import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MovimientoService } from 'src/app/core/services/movimiento.service';
import { CuentaService } from 'src/app/core/services/cuenta.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { Movimiento } from 'src/app/core/models/Movimiento';
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
  public movimientoForm!: FormGroup;
  public formSubmitted = false;
  public isEditMode = false;
  public editingMovimientoId: number | null = null;

  // Propiedades de datos
  public movimientos: MovimientoDto[] = [];
  public cuentas: CuentaDto[] = [];
  public filteredItems: MovimientoDto[] = [];
  public paginatedItems: MovimientoDto[] = [];

  // Variables de búsqueda y filtrado
  public searchTerm: string = '';

  // Variables de paginación
  public currentPage = 1;
  public pageSize = 5;
  public totalPages = 0;
  public pages: number[] = [];
  public startIndex = 0;
  public endIndex = 0;

  // Modal states
  public isModalVisible = false;
  public isDeleteModalVisible = false;
  public movimientoToDelete: MovimientoDto | null = null;

  // Enums para el template
  public TipoMovimiento = TipoMovimiento;

  constructor(
    private movimientoService: MovimientoService,
    private cuentaService: CuentaService,
    private fb: FormBuilder,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.initMovimientoForm();
    this.cargarMovimientos();
    this.cargarCuentas();
  }

  // === FORM SETUP ===
  private initMovimientoForm(): void {
    this.movimientoForm = this.fb.group({
      id: [null],
      cuentaId: ['', [Validators.required]],
      tipoMovimiento: ['', [Validators.required]],
      valor: ['', [Validators.required, Validators.min(0.01)]]
    });
  }

  // --- Métodos de carga de datos ---

  private cargarMovimientos(): void {
    this.movimientoService.getMovimientos().subscribe({
      next: (response: any) => {
        if (response.success) {
          this.movimientos = response.data;
          this.enrichMovimientosConCuentas(); // Enriquecer con información de cuentas
          this.filteredItems = [...this.movimientos];
          this.updatePagination();
          this.updatePage();
        } else {
          this.notificationService.showError('Error al cargar movimientos: ' + (response.errors || 'Error desconocido'));
        }
      },
      error: (error) => {
        this.notificationService.showError('Error al conectar con el servidor para cargar movimientos');
      }
    });
  }

  private cargarCuentas(): void {
    this.cuentaService.getCuentas().subscribe({
      next: (response: any) => {
        if (response.success) {
          this.cuentas = response.data;
          this.enrichMovimientosConCuentas(); // Enriquecer movimientos si ya están cargados
        } else {
          this.notificationService.showError('Error al cargar cuentas: ' + (response.errors || 'Error desconocido'));
        }
      },
      error: (error) => {
        this.notificationService.showError('Error al conectar con el servidor para cargar cuentas');
      }
    });
  }

  // Método para enriquecer los movimientos con información completa de las cuentas
  private enrichMovimientosConCuentas(): void {
    if (this.movimientos.length > 0 && this.cuentas.length > 0) {
      this.movimientos.forEach(movimiento => {
        if (movimiento.cuenta && movimiento.cuenta.id) {
          const cuentaCompleta = this.cuentas.find(c => c.id === movimiento.cuenta.id);
          if (cuentaCompleta) {
            movimiento.cuenta.numeroCuenta = cuentaCompleta.numeroCuenta || '';
          }
        }
      });
    }
  }

  // --- Métodos CRUD ---

  public saveMovimiento(): void {
    console.log('=== INICIANDO saveMovimiento ===');
    this.formSubmitted = true;

    console.log('Formulario válido:', this.movimientoForm.valid);
    console.log('Valores del formulario:', this.movimientoForm.value);

    if (this.movimientoForm.invalid) {
      console.log('Formulario inválido, errores:', this.movimientoForm.errors);
      this.notificationService.showWarning('Por favor, complete todos los campos requeridos correctamente');
      this.markFormGroupTouched(this.movimientoForm);
      return;
    }

    try {
      console.log('Creando DTO...');
      const movimientoDto = this.createMovimientoDtoFromForm();
      console.log('DTO creado exitosamente:', movimientoDto);

      if (this.isEditMode && this.editingMovimientoId) {
        console.log('Modo edición');
        this.updateMovimiento(this.editingMovimientoId, movimientoDto);
      } else {
        console.log('Modo creación');
        this.createMovimiento(movimientoDto);
      }
    } catch (error: any) {
      console.error('Error capturado en saveMovimiento:', error);
      this.handleError('Error en el formulario', { error: { message: error.message } });
    }
  }

  private createMovimiento(movimientoDto: MovimientoDto): void {
    console.log('Enviando movimiento:', JSON.stringify(movimientoDto, null, 2));
    this.movimientoService.createMovimiento(movimientoDto).subscribe({
      next: (response: any) => {
        console.log('Respuesta del servidor:', response);
        if (response.success) {
          this.handleSuccess('Movimiento creado exitosamente');
          this.resetForm();
          this.cargarMovimientos();
        } else {
          this.handleError('Error al crear el movimiento', response);
        }
      },
      error: (error) => {
        console.error('Error completo:', error);
        this.handleError('Error al crear el movimiento', error);
      }
    });
  }

  private updateMovimiento(id: number, movimientoDto: MovimientoDto): void {
    this.movimientoService.updateMovimiento(id, movimientoDto).subscribe({
      next: (response: any) => {
        if (response.success) {
          this.handleSuccess('Movimiento actualizado exitosamente');
          this.resetForm();
          this.cargarMovimientos();
        } else {
          this.handleError('Error al actualizar el movimiento', response);
        }
      },
      error: (error) => {
        this.handleError('Error al actualizar el movimiento', error);
      }
    });
  }

  private deleteMovimiento(): void {
    if (!this.movimientoToDelete || !this.movimientoToDelete.id) {
      return;
    }

    this.movimientoService.deleteMovimiento(this.movimientoToDelete.id).subscribe({
      next: (response: any) => {
        if (response.success) {
          this.handleSuccess('Movimiento eliminado exitosamente');
          this.closeDeleteModal();
          this.cargarMovimientos();
        } else {
          this.handleError('Error al eliminar el movimiento', response);
          this.closeDeleteModal();
        }
      },
      error: (error) => {
        this.handleError('Error al eliminar el movimiento', error);
        this.closeDeleteModal();
      }
    });
  }

  // === MODAL METHODS ===
  public openCreateModal(): void {
    this.isModalVisible = true;
    this.isEditMode = false;
    this.editingMovimientoId = null;
    this.formSubmitted = false;
    this.movimientoForm.reset();
    this.movimientoForm.patchValue({
      id: null,
      cuentaId: '',
      tipoMovimiento: '',
      valor: ''
    });
  }

  public openEditModal(movimiento: MovimientoDto): void {
    if (movimiento && movimiento.id && movimiento.cuenta && movimiento.valor !== undefined) {
      this.isModalVisible = true;
      this.isEditMode = true;
      this.editingMovimientoId = movimiento.id;
      this.formSubmitted = false;

      this.movimientoForm.patchValue({
        id: movimiento.id,
        cuentaId: movimiento.cuenta.id,
        tipoMovimiento: movimiento.tipoMovimiento,
        valor: Math.abs(movimiento.valor)
      });
    }
  }

  public closeModal(): void {
    this.isModalVisible = false;
    this.resetForm();
  }

  public openDeleteModal(movimiento: MovimientoDto): void {
    this.movimientoToDelete = movimiento;
    this.isDeleteModalVisible = true;
  }

  public closeDeleteModal(): void {
    this.isDeleteModalVisible = false;
    this.movimientoToDelete = null;
  }

  public confirmDelete(): void {
    this.deleteMovimiento();
  }

  // --- Métodos auxiliares ---

  private resetForm(): void {
    this.isModalVisible = false;
    this.isEditMode = false;
    this.editingMovimientoId = null;
    this.formSubmitted = false;
    this.movimientoForm.reset();
  }

  private createMovimientoDtoFromForm(): MovimientoDto {
    const formValue = this.movimientoForm.value;
    console.log('Form value:', formValue);
    console.log('Cuentas disponibles:', this.cuentas);

    const cuentaId = parseInt(formValue.cuentaId);
    console.log('CuentaId parseado:', cuentaId);

    const selectedCuenta = this.cuentas.find(c => c.id === cuentaId);
    console.log('Cuenta encontrada:', selectedCuenta);

    if (!selectedCuenta) {
      throw new Error(`No se encontró la cuenta con ID: ${cuentaId}`);
    }

    const movimientoDto = new MovimientoDto();

    // ID del movimiento (0 para crear, el ID actual para editar)
    movimientoDto.id = this.isEditMode ? this.editingMovimientoId! : 0;

    // Fecha actual
    movimientoDto.fecha = new Date();

    // Tipo de movimiento (enum)
    movimientoDto.tipoMovimiento = parseInt(formValue.tipoMovimiento) as TipoMovimiento;

    // Valor del movimiento (siempre positivo desde el form)
    movimientoDto.valor = parseFloat(formValue.valor);

    // Saldo inicial en 0 - el backend lo calculará
    movimientoDto.saldo = 0;

    // Información de la cuenta
    movimientoDto.cuenta = new MovimientoDto.CuentaResumida();
    movimientoDto.cuenta.id = selectedCuenta.id!;
    movimientoDto.cuenta.numeroCuenta = selectedCuenta.numeroCuenta!;

    console.log('DTO final antes de enviar:', JSON.stringify(movimientoDto, null, 2));

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

  private handleSuccess(message: string): void {
    console.log(message);
    this.notificationService.showSuccess(message);
  }

  private handleError(message: string, error: any): void {
    console.error(message, error);
    let errorMessage = message;
    if (error?.error?.message) {
      errorMessage = error.error.message;
    }
    this.notificationService.showError(errorMessage);
  }

  // Filtrar elementos según término de búsqueda
  public filterItems(): void {
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

  // Actualizar información de paginación
  updatePagination() {
    this.totalPages = Math.ceil(this.filteredItems.length / this.pageSize);
    this.generatePages();
  }

  // Generar array de números de página
  generatePages() {
    this.pages = [];
    for (let i = 1; i <= this.totalPages; i++) {
      this.pages.push(i);
    }
  }

  // Cambiar a la página seleccionada
  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePage();
    }
  }

  // Actualizar elementos mostrados
  updatePage() {
    this.startIndex = (this.currentPage - 1) * this.pageSize;
    this.endIndex = Math.min(this.startIndex + this.pageSize, this.filteredItems.length);
    this.paginatedItems = this.filteredItems.slice(this.startIndex, this.endIndex);
  }

  // === NOTIFICATION EXAMPLES ===
  // Métodos de ejemplo para demostrar el uso de las notificaciones
  showSuccessExample(): void {
    this.notificationService.showSuccess('¡Operación exitosa en movimientos!');
  }

  showErrorExample(): void {
    this.notificationService.showError('Ha ocurrido un error en movimientos');
  }

  showWarningExample(): void {
    this.notificationService.showWarning('Advertencia: revise los datos del movimiento');
  }

  showInfoExample(): void {
    this.notificationService.showWarning('Información importante sobre movimientos');
  }
}
