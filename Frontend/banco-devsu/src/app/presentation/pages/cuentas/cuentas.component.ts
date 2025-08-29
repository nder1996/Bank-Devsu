import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CuentaDto } from 'src/app/core/dtos/cuenta.dto';
import { ClienteDto } from 'src/app/core/dtos/cliente.dto';
import { Cuenta } from 'src/app/core/models/Cuenta';
import { CuentaService } from 'src/app/core/services/cuenta.service';
import { ClienteService } from 'src/app/core/services/cliente.service';


@Component({
  selector: 'app-cuentas',
  templateUrl: './cuentas.component.html',
  styleUrls: ['./cuentas.component.css']
})
export class CuentasComponent implements OnInit {
  items = [
    { fecha: '10/08/2025', cuenta: '478758', tipo: 'Crédito', valor: 2000, saldo: 4500 },
    { fecha: '08/08/2025', cuenta: '225487', tipo: 'Débito', valor: -575, saldo: 2500 },
    { fecha: '05/08/2025', cuenta: '495878', tipo: 'Crédito', valor: 600, saldo: 700 },
    { fecha: '01/08/2025', cuenta: '496825', tipo: 'Débito', valor: -150, saldo: 0 },
    { fecha: '30/07/2025', cuenta: '478758', tipo: 'Crédito', valor: 1500, saldo: 3000 },
    { fecha: '28/07/2025', cuenta: '225487', tipo: 'Débito', valor: -800, saldo: 3075 },
    { fecha: '26/07/2025', cuenta: '495878', tipo: 'Crédito', valor: 100, saldo: 100 },
    { fecha: '20/07/2025', cuenta: '496825', tipo: 'Débito', valor: -250, saldo: 150 },
    { fecha: '15/07/2025', cuenta: '478758', tipo: 'Crédito', valor: 1000, saldo: 1500 },
    { fecha: '10/07/2025', cuenta: '225487', tipo: 'Débito', valor: -125, saldo: 3875 },
    { fecha: '05/07/2025', cuenta: '495878', tipo: 'Crédito', valor: 900, saldo: 1000 },
    { fecha: '01/07/2025', cuenta: '496825', tipo: 'Débito', valor: -300, saldo: 400 }
  ];

  cuentas: CuentaDto[] = [];
  cuentasTransformadas: any[] = [];
  clientes: ClienteDto[] = [];

  searchTerm: string = '';
  filteredItems: any[] = [];

  paginatedItems: any[] = [];
  currentPage = 1;
  pageSize = 5;
  totalPages = 0;
  pages: number[] = [];
  startIndex = 0;
  endIndex = 0;

  constructor(private cuentaService: CuentaService, private clienteService: ClienteService, private fb: FormBuilder) { }

  clientForm!: FormGroup;
  accountForm!: FormGroup;
  isEditMode = false;
  formSubmitted = false; // Variable para controlar si el formulario ha sido enviado
  isDialogVisible = false;
  isDeleteModalVisible = false;
  clientToDelete: any = null;
  accountToDelete: any = null;

  saveClient(): void {
    this.formSubmitted = true;
    if (this.clientForm.valid) {
      // Aquí iría el código para guardar el cliente
      console.log('Formulario enviado:', this.clientForm.value);
      this.isDialogVisible = false;
      this.formSubmitted = false;
      this.clientForm.reset();
    }
  }

  saveAccount(): void {
    this.formSubmitted = true;
    if (this.accountForm.valid) {
      // Crear el DTO con los datos del formulario
      const cuentaDto: CuentaDto = {
        numeroCuenta: this.accountForm.get('numeroCuenta')?.value,
        tipoCuenta: this.accountForm.get('tipoCuenta')?.value,
        saldoInicial: this.accountForm.get('saldoInicial')?.value,
        estado: this.accountForm.get('estado')?.value,
        cliente: {
          id: this.accountForm.get('clienteId')?.value
        }
      };

      console.log('Cuenta a guardar:', JSON.stringify(cuentaDto));

      // Consumir el servicio para crear la cuenta
      this.cuentaService.createCuenta(cuentaDto).subscribe(
        (response: any) => {
          console.log('Respuesta del servidor:', response);

          // Verificar si la respuesta tiene el formato estándar
          if (response.success || response.id || response.numeroCuenta) {
            console.log('Cuenta creada exitosamente');

            // Cerrar el modal y resetear el formulario
            this.isDialogVisible = false;
            this.formSubmitted = false;
            this.accountForm.reset({
              estado: true
            });

            // Recargar las cuentas después de guardar
            this.cargarCuentas();

            // Mostrar mensaje de éxito (opcional)
            alert('Cuenta creada exitosamente');
          } else {
            console.error('Error en la respuesta del servidor:', response);
            alert('Error al crear la cuenta. Verifique los datos e intente de nuevo.');
          }
        },
        (error) => {
          console.error('Error al crear la cuenta:', error);

          // Mostrar mensaje de error más específico
          let errorMessage = 'Error al crear la cuenta. Por favor intente de nuevo.';
          if (error.error && error.error.message) {
            errorMessage = error.error.message;
          } else if (error.message) {
            errorMessage = error.message;
          }

          alert(errorMessage);
        }
      );
    } else {
      console.log('Formulario inválido:', this.accountForm.errors);
      // Marcar todos los campos como tocados para mostrar errores
      Object.keys(this.accountForm.controls).forEach(key => {
        this.accountForm.get(key)?.markAsTouched();
      });
    }
  }

  cancelForm(): void {
    this.isDialogVisible = false;
    this.formSubmitted = false;
    this.clientForm.reset();
    this.accountForm.reset();
  }

  ngOnInit() {
    this.initForm();
    this.initAccountForm();
    this.cargarCuentas();
    this.cargarClientes();
  }

  cargarCuentas(): void {
    this.cuentaService.getCuentas().subscribe(
      (response: any) => {
        if (response.success) {
          this.cuentas = response.data;
          this.cuentasTransformadas = response.data.map((cuenta: CuentaDto) => ({
            cuentaId: cuenta.numeroCuenta,
            tipo: cuenta.tipoCuenta === 1 ? 'Crédito' : 'Débito',
            //valor: cuenta.saldoInicial,
            saldo: cuenta.saldoInicial,
            estado: cuenta.estado,
            nombreCliente: cuenta.cliente?.nombre || '',
            /*fecha: new Date(cuenta.).toLocaleDateString('es-ES', {
              year: 'numeric',
              month: '2-digit',
              day: '2-digit'
            })*/
          }));
          console.log("json cuentas :", JSON.stringify(this.cuentasTransformadas));
          this.filteredItems = [...this.cuentasTransformadas];
          this.updatePagination();
          this.updatePage();
        } else {
          console.error('Errores en la respuesta:', response.errors);
        }
      },
      (error) => {
        console.error('Error al cargar cuentas:', error);
      }
    );
  }

  cargarClientes(): void {
    this.clienteService.getClientes().subscribe(
      (response: any) => {
        if (response.success) {
          this.clientes = response.data;
        } else {
          console.error('Errores en la respuesta:', response.errors);
        }
      },
      (error) => {
        console.error('Error al cargar clientes:', error);
      }
    );
  }

  filterItems() {
    if (!this.searchTerm.trim()) {
      this.filteredItems = [...this.cuentasTransformadas];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredItems = this.cuentasTransformadas.filter(item =>
        (item.cuentaId && item.cuentaId.toString().toLowerCase().includes(term)) ||
        (item.tipo && item.tipo.toString().toLowerCase().includes(term)) ||
        (item.saldo && item.saldo.toString().includes(term)) ||
        (item.estado !== undefined && item.estado.toString().toLowerCase().includes(term)) ||
        (item.nombreCliente && item.nombreCliente.toString().toLowerCase().includes(term))
      );
    }

    this.currentPage = 1;
    this.updatePagination();
    this.updatePage();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredItems.length / this.pageSize);
    this.generatePages();
  }

  generatePages() {
    this.pages = [];
    for (let i = 1; i <= this.totalPages; i++) {
      this.pages.push(i);
    }
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePage();
    }
  }

  updatePage() {
    this.startIndex = (this.currentPage - 1) * this.pageSize;
    this.endIndex = Math.min(this.startIndex + this.pageSize, this.filteredItems.length);
    this.paginatedItems = this.filteredItems.slice(this.startIndex, this.endIndex);
  }

  showDialog(isEdit: boolean = false, clientData?: any): void {
    this.isEditMode = isEdit;
    this.formSubmitted = false;

    if (isEdit && clientData) {
      this.clientForm.patchValue(clientData);
    } else {
      this.clientForm.reset({
        estado: true
      });
    }

    this.isDialogVisible = true;
  }

  showAccountDialog(isEdit: boolean = false, accountData?: any): void {
    this.isEditMode = isEdit;
    this.formSubmitted = false;

    if (isEdit && accountData) {
      this.accountForm.patchValue(accountData);
    } else {
      this.accountForm.reset({
        estado: true
      });
    }

    this.isDialogVisible = true;
  }

  initForm(): void {
    this.clientForm = this.fb.group({
      clienteid: [null],
      nombre: ['', Validators.required],
      genero: ['', Validators.required],
      edad: ['', [Validators.required, Validators.min(18)]],
      identificacion: ['', Validators.required],
      direccion: ['', Validators.required],
      telefono: ['', Validators.required],
      contrasena: ['', [Validators.required, Validators.minLength(6)]],
      estado: [true, Validators.required]
    });
  }

  initAccountForm(): void {
    this.accountForm = this.fb.group({
      numeroCuenta: ['', Validators.required],
      tipoCuenta: ['', Validators.required],
      saldoInicial: ['', [Validators.required, Validators.min(0)]],
      clienteId: ['', Validators.required],
      estado: [true, Validators.required]
    });
  }

  // Métodos para el modal de eliminación
  showDeleteConfirmation(client: any): void {
    this.clientToDelete = client;
    this.isDeleteModalVisible = true;
  }

  showDeleteConfirmationAccount(account: any): void {
    this.accountToDelete = account;
    this.isDeleteModalVisible = true;
  }

  cancelDelete(): void {
    this.clientToDelete = null;
    this.accountToDelete = null;
    this.isDeleteModalVisible = false;
  }

  deleteClient(): void {
    if (this.clientToDelete) {
      // Aquí iría la lógica para eliminar el cliente
      console.log('Eliminando cliente:', this.clientToDelete);

      // Simulación de eliminación exitosa
      this.isDeleteModalVisible = false;
      this.clientToDelete = null;

      // Actualizar la lista de clientes
      this.cargarCuentas();
    }
  }

  deleteAccount(): void {
    if (this.accountToDelete) {
      // Aquí iría la lógica para eliminar la cuenta
      console.log('Eliminando cuenta:', this.accountToDelete);

      // Simulación de eliminación exitosa
      this.isDeleteModalVisible = false;
      this.accountToDelete = null;

      // Actualizar la lista de cuentas
      this.cargarCuentas();
    }
  }







}
