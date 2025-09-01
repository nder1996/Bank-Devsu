import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClienteDto } from 'src/app/core/dtos/cliente.dto';
import { ClienteService } from '../../../core/services/cliente.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent implements OnInit {
  // Data properties
  clientes: ClienteDto[] = [];
  filteredClientes: ClienteDto[] = [];

  // Pagination
  paginatedClientes: ClienteDto[] = [];
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
  clienteToDelete: ClienteDto | null = null;

  // Form
  clientForm!: FormGroup;
  formSubmitted = false;

  constructor(
    private clienteService: ClienteService,
    private fb: FormBuilder,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadClientes();
  }

  // === FORM SETUP ===
  private initForm(): void {
    this.clientForm = this.fb.group({
      id: [null],
      nombre: ['', Validators.required],
      genero: ['', Validators.required],
      edad: ['', [Validators.required, Validators.min(18)]],
      identificacion: ['', Validators.required],
      direccion: ['', Validators.required],
      telefono: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      contrasena: ['', [Validators.minLength(6)]],
      estado: [true, Validators.required]
    });
  }

  private updatePasswordValidation(): void {
    const passwordControl = this.clientForm.get('contrasena');
    if (this.isEditMode) {
      passwordControl?.setValidators([Validators.minLength(6)]);
    } else {
      passwordControl?.setValidators([Validators.required, Validators.minLength(6)]);
    }
    passwordControl?.updateValueAndValidity();
  }

  // === DATA LOADING ===
  private loadClientes(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.clienteService.getClientes().subscribe({
      next: (response: any) => {
        if (response.success) {
          this.clientes = this.mapClientesFromResponse(response.data);
          this.applyFilter();
        } else {
          this.notificationService.showError(
            'Error al cargar clientes: ' + (response.errors || 'Error desconocido')
          );
        }
      },
      error: () => {
        this.notificationService.showError('Error al conectar con el servidor');
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  private mapClientesFromResponse(data: any[]): ClienteDto[] {
    return data.map(item => new ClienteDto(
      item.id,
      item.nombre,
      item.genero,
      item.edad,
      item.identificacion,
      item.direccion,
      item.telefono,
      item.clienteId || item.id,
      item.contrasena || '',
      item.estado,
      item.cuentas || []
    ));
  }

  // === SEARCH & PAGINATION ===
  onSearch(): void {
    this.currentPage = 1;
    this.applyFilter();
  }

  private applyFilter(): void {
    if (!this.searchTerm.trim()) {
      this.filteredClientes = [...this.clientes];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredClientes = this.clientes.filter(cliente =>
        this.searchInCliente(cliente, term)
      );
    }
    this.updatePagination();
  }

  private searchInCliente(cliente: ClienteDto, term: string): boolean {
    return (
      (cliente.nombre?.toLowerCase().includes(term) ?? false) ||
      (cliente.identificacion?.toLowerCase().includes(term) ?? false) ||
      (cliente.direccion?.toLowerCase().includes(term) ?? false) ||
      (cliente.telefono?.toLowerCase().includes(term) ?? false) ||
      (cliente.genero?.toLowerCase().includes(term) ?? false) ||
      (cliente.edad?.toString().includes(term) ?? false)
    );
  }

  private updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredClientes.length / this.pageSize);
    this.generatePageNumbers();
    this.updateCurrentPage();
  }

  private generatePageNumbers(): void {
    this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  private updateCurrentPage(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedClientes = this.filteredClientes.slice(startIndex, endIndex);
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
    this.updatePasswordValidation();
    this.isModalVisible = true;
  }

  openEditModal(cliente: ClienteDto): void {
    this.resetModal();
    this.isEditMode = true;
    this.updatePasswordValidation();

    // Load client data into form
    this.clientForm.patchValue({
      id: cliente.id,
      nombre: cliente.nombre,
      genero: cliente.genero,
      edad: cliente.edad,
      identificacion: cliente.identificacion,
      direccion: cliente.direccion,
      telefono: cliente.telefono,
      contrasena: '', // Empty for security
      estado: cliente.estado
    });

    this.isModalVisible = true;
  }

  closeModal(): void {
    this.isModalVisible = false;
    this.resetModal();
  }

  private resetModal(): void {
    this.clientForm.reset();
    this.clientForm.patchValue({ estado: true });
    this.formSubmitted = false;
  }

  // === CRUD OPERATIONS ===
  saveCliente(): void {
    this.formSubmitted = true;

    // Mark all fields as touched for validation display
    Object.keys(this.clientForm.controls).forEach(key => {
      this.clientForm.get(key)?.markAsTouched();
    });

    if (this.clientForm.invalid) {
      this.notificationService.showWarning('Por favor, complete todos los campos requeridos correctamente');
      return;
    }

    const formData = this.clientForm.value;

    // Crear el DTO completo en el componente
    const clienteDto = new ClienteDto(
      formData.id || 0,
      formData.nombre,
      formData.genero,
      formData.edad,
      formData.identificacion,
      formData.direccion,
      formData.telefono,
      formData.clienteId || formData.id || 0,
      formData.contrasena,
      formData.estado,
      []
    );

    const operation = this.isEditMode
      ? this.clienteService.updateCliente(clienteDto.id!, clienteDto)
      : this.clienteService.createCliente(clienteDto);

    operation.subscribe({
      next: () => {
        this.closeModal();
        this.loadClientes();
        // Mostrar notificación de éxito
        const action = this.isEditMode ? 'actualizado' : 'creado';
        this.notificationService.showSuccess(`Cliente ${action} exitosamente`);
      },
      error: (error) => {
        console.error('Error al guardar cliente:', error);
        const action = this.isEditMode ? 'actualizar' : 'crear';
        this.notificationService.showError(`Error al ${action} el cliente`);
      }
    });
  }

  // === DELETE OPERATIONS ===
  openDeleteModal(cliente: ClienteDto): void {
    this.clienteToDelete = cliente;
    this.isDeleteModalVisible = true;
  }

  closeDeleteModal(): void {
    this.clienteToDelete = null;
    this.isDeleteModalVisible = false;
  }

  confirmDelete(): void {
    if (!this.clienteToDelete) return;

    this.clienteService.deleteCliente(this.clienteToDelete.id!).subscribe({
      next: () => {
        this.closeDeleteModal();
        this.loadClientes();
        this.notificationService.showSuccess('Cliente eliminado exitosamente');
      },
      error: (error) => {
        console.error('Error al eliminar cliente:', error);
        this.notificationService.showError('Error al eliminar el cliente');
        this.closeDeleteModal();
      }
    });
  }

  // === UTILITY GETTERS ===
  get startIndex(): number {
    return this.filteredClientes.length > 0 ? (this.currentPage - 1) * this.pageSize + 1 : 0;
  }

  get endIndex(): number {
    return Math.min(this.currentPage * this.pageSize, this.filteredClientes.length);
  }

  get hasData(): boolean {
    return this.paginatedClientes.length > 0;
  }

  get showNoData(): boolean {
    return !this.isLoading && !this.hasData;
  }

  // === NOTIFICATION EXAMPLES ===
  // Métodos de ejemplo para demostrar el uso de las notificaciones
  showSuccessExample(): void {
    this.notificationService.showSuccess('¡Operación exitosa!');
  }

  showErrorExample(): void {
    this.notificationService.showError('Ha ocurrido un error');
  }

  showWarningExample(): void {
    this.notificationService.showWarning('Advertencia: revise los datos');
  }

  showInfoExample(): void {
    this.notificationService.showWarning('Información importante');
  }
}
