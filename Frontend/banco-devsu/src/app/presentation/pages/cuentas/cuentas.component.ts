import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CuentaDto } from 'src/app/core/dtos/cuenta.dto';
import { ClienteDto } from 'src/app/core/dtos/cliente.dto';
import { CuentaService } from 'src/app/core/services/cuenta.service';
import { ClienteService } from 'src/app/core/services/cliente.service';
import { TipoCuenta } from 'src/app/core/enums/tipo-cuenta.enum';

interface CuentaViewModel {
  id: number;
  cuentaId: string;
  tipo: string;
  saldo: number;
  estado: boolean;
  nombreCliente: string;
}

@Component({
  selector: 'app-cuentas',
  templateUrl: './cuentas.component.html',
  styleUrls: ['./cuentas.component.css']
})
export class CuentasComponent implements OnInit {

  // Propiedades del formulario
  public accountForm!: FormGroup;
  public formSubmitted = false;
  public isEditMode = false;
  public editingAccountId: number | null = null;

  // Propiedades de datos
  public cuentas: CuentaViewModel[] = [];
  public clientes: ClienteDto[] = [];
  public filteredCuentas: CuentaViewModel[] = [];
  public paginatedCuentas: CuentaViewModel[] = [];

  // Propiedades de paginación y búsqueda
  public searchTerm: string = '';
  public currentPage = 1;
  public pageSize = 5;
  public totalPages = 0;
  public pages: number[] = [];
  public startIndex = 0;
  public endIndex = 0;

  // Propiedades de modales
  public isDialogVisible = false;
  public isDeleteModalVisible = false;
  public accountToDelete: CuentaViewModel | null = null;

  constructor(
    private cuentaService: CuentaService,
    private clienteService: ClienteService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.initAccountForm();
    this.loadCuentas();
    this.loadClientes();
  }

  // --- Métodos Públicos (Accesibles desde la plantilla) ---

  public saveAccount(): void {
    this.formSubmitted = true;
    if (this.accountForm.invalid) {
      this.markFormGroupTouched(this.accountForm);
      return;
    }

    const cuentaDto = this.createCuentaDtoFromForm();
    
    if (this.isEditMode && this.editingAccountId) {
      this.updateAccount(this.editingAccountId, cuentaDto);
    } else {
      this.createAccount(cuentaDto);
    }
  }

  public deleteAccount(): void {
    if (!this.accountToDelete) {
      return;
    }

    this.cuentaService.deleteCuenta(this.accountToDelete.id).subscribe({
      next: () => {
        this.handleSuccess('Cuenta eliminada exitosamente');
        this.isDeleteModalVisible = false;
        this.accountToDelete = null;
        this.loadCuentas();
      },
      error: (error) => {
        this.handleError('Error al eliminar la cuenta', error);
        this.isDeleteModalVisible = false;
        this.accountToDelete = null;
      }
    });
  }

  public filterCuentas(): void {
    const term = this.searchTerm.toLowerCase().trim();
    this.filteredCuentas = term
      ? this.cuentas.filter(cuenta =>
        Object.values(cuenta).some(value =>
          String(value).toLowerCase().includes(term)
        )
      )
      : [...this.cuentas];

    this.currentPage = 1;
    this.updatePagination();
  }

  public changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePage();
    }
  }

  public showAccountDialog(isEdit: boolean = false, accountData?: CuentaViewModel): void {
    this.isEditMode = isEdit;
    this.formSubmitted = false;
    this.isDialogVisible = true;

    if (isEdit && accountData) {
      this.editingAccountId = accountData.id;
      const clienteId = this.getClienteIdByName(accountData.nombreCliente);
      
      this.accountForm.patchValue({
        numeroCuenta: accountData.cuentaId,
        tipoCuenta: accountData.tipo,
        saldoInicial: accountData.saldo,
        clienteId: clienteId,
        estado: accountData.estado
      });
    } else {
      this.editingAccountId = null;
      this.accountForm.reset({ estado: true });
    }
  }

  public showDeleteConfirmation(account: CuentaViewModel): void {
    this.accountToDelete = account;
    this.isDeleteModalVisible = true;
  }

  public cancelDelete(): void {
    this.accountToDelete = null;
    this.isDeleteModalVisible = false;
  }

  public cancelForm(): void {
    this.isDialogVisible = false;
    this.formSubmitted = false;
    this.isEditMode = false;
    this.editingAccountId = null;
    this.accountForm.reset();
  }

  // --- Métodos Privados (Lógica interna del componente) ---

  private initAccountForm(): void {
    this.accountForm = this.fb.group({
      numeroCuenta: ['', Validators.required],
      tipoCuenta: ['', Validators.required],
      saldoInicial: ['', [Validators.required, Validators.min(0)]],
      clienteId: ['', Validators.required],
      estado: [true, Validators.required]
    });
  }

  private loadCuentas(): void {
    this.cuentaService.getCuentas().subscribe({
      next: (response: any) => {
        if (response.success) {
          this.cuentas = response.data.map(this.transformCuentaToViewModel);
          this.filteredCuentas = [...this.cuentas];
          this.updatePagination();
        } else {
          this.handleError('Errores en la respuesta al cargar cuentas', response.errors);
        }
      },
      error: (error) => this.handleError('Error al cargar cuentas', error)
    });
  }

  private loadClientes(): void {
    this.clienteService.getClientes().subscribe({
      next: (response: any) => {
        if (response.success) {
          this.clientes = response.data;
        } else {
          this.handleError('Errores en la respuesta al cargar clientes', response.errors);
        }
      },
      error: (error) => this.handleError('Error al cargar clientes', error)
    });
  }

  private transformCuentaToViewModel(cuenta: any): CuentaViewModel {
    return {
      id: cuenta.id || 0,
      cuentaId: cuenta.numeroCuenta || '',
      tipo: cuenta.tipoCuenta === TipoCuenta.Ahorro ? 'Ahorro' : 'Corriente',
      saldo: cuenta.saldoInicial || cuenta.saldo || 0,
      estado: cuenta.estado || false,
      nombreCliente: cuenta.cliente?.nombre || 'N/A',
    };
  }

  private createCuentaDtoFromForm(): CuentaDto {
    const formValue = this.accountForm.value;
    const clienteSeleccionado = this.clientes.find(c => c.id === formValue.clienteId);
    
    return {
      numeroCuenta: formValue.numeroCuenta,
      tipoCuenta: formValue.tipoCuenta === 'Ahorro' ? TipoCuenta.Ahorro : TipoCuenta.Corriente,
      saldoInicial: formValue.saldoInicial,
      estado: formValue.estado,
      cliente: {
        id: formValue.clienteId,
        nombre: clienteSeleccionado?.nombre || 'Cliente no encontrado'
      }
    };
  }

  private updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredCuentas.length / this.pageSize);
    this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
    this.updatePage();
  }

  private updatePage(): void {
    this.startIndex = (this.currentPage - 1) * this.pageSize;
    this.endIndex = Math.min(this.startIndex + this.pageSize, this.filteredCuentas.length);
    this.paginatedCuentas = this.filteredCuentas.slice(this.startIndex, this.endIndex);
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  private handleSuccess(message: string): void {
    console.log(message);
    alert(message); // Reemplazar con un servicio de notificaciones
    this.isDialogVisible = false;
    this.formSubmitted = false;
    this.accountForm.reset({ estado: true });
  }

  private handleError(message: string, error?: any): void {
    console.error(message, error);
    const errorMessage = error?.error?.message || error?.message || 'Ocurrió un error inesperado.';
    alert(errorMessage); // Reemplazar con un servicio de notificaciones
  }

  private createAccount(cuentaDto: CuentaDto): void {
    this.cuentaService.createCuenta(cuentaDto).subscribe({
      next: (response: any) => {
        if (response.success || response.id || response.numeroCuenta) {
          this.handleSuccess('Cuenta creada exitosamente');
          this.loadCuentas();
        } else {
          this.handleError('Error en la respuesta del servidor', response);
        }
      },
      error: (error) => this.handleError('Error al crear la cuenta', error)
    });
  }

  private updateAccount(id: number, cuentaDto: CuentaDto): void {
    this.cuentaService.updateCuenta(id, cuentaDto).subscribe({
      next: (response: any) => {
        if (response.success || response.id || response.numeroCuenta) {
          this.handleSuccess('Cuenta actualizada exitosamente');
          this.loadCuentas();
        } else {
          this.handleError('Error en la respuesta del servidor', response);
        }
      },
      error: (error) => this.handleError('Error al actualizar la cuenta', error)
    });
  }


  private getClienteIdByName(nombreCliente: string): number | null {
    const cliente = this.clientes.find(c => c.nombre === nombreCliente);
    return cliente?.id || null;
  }
}
