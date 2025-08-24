import { Component, ElementRef, ViewChild } from '@angular/core';
import { CuentaDto } from 'src/app/core/dtos/cuenta.dto';
import { CuentaService } from 'src/app/core/services/cuenta.service';

import { ClienteDto } from 'src/app/core/dtos/cliente.dto';
import { ClienteService } from '../../../core/services/cliente.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-clientes',
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css']
})
export class ClientesComponent {
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

  constructor(private clienteService:ClienteService, private fb: FormBuilder) { }

  clientForm!: FormGroup;
  isEditMode = false;
  formSubmitted = false; // Variable para controlar si el formulario ha sido enviado

  ngOnInit() {
    this.initForm();
    this.cargarCuentas();
  }

  cargarCuentas(): void {
    this.clienteService.getClientes().subscribe(
      (response: any) => {
        if (response.success) {
          // Mapeamos los clientes y sus cuentas para el CRUD
          this.clientes = response.data.map((cliente: ClienteDto) => ({
            id: cliente.id,
            nombre: cliente.nombre,
            identificacion: cliente.identificacion,
            direccion: cliente.direccion,
            telefono: cliente.telefono,
            edad: cliente.edad,
            genero: cliente.genero,
            estado: cliente.estado,
            cuentas: cliente.cuentas?.map((cuenta: CuentaDto) => ({
              cuentaId: cuenta.numeroCuenta,
              tipo: cuenta.tipoCuenta === 1 ? 'Crédito' : 'Débito',
              saldo: cuenta.saldoInicial,
              estado: cuenta.estado
            })) || []
          }));
          console.log("json clientes :", JSON.stringify(response.data));
          this.filteredItems = [...this.clientes];
          this.updatePagination();
          this.updatePage();
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
      this.filteredItems = [...this.items];
    } else {
      const term = this.searchTerm.toLowerCase();
      this.filteredItems = this.items.filter(item =>
        item.cuenta.toLowerCase().includes(term) ||
        item.tipo.toLowerCase().includes(term) ||
        item.fecha.includes(term) ||
        item.valor.toString().includes(term) ||
        item.saldo.toString().includes(term)
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

  isDialogVisible = false;

  showDialog(): void {
    // Limpiar el formulario para un nuevo cliente
    this.clientForm.reset();
    // Establecer el valor por defecto para estado
    this.clientForm.get('estado')?.setValue(true);
    // Resetear el modo de edición
    this.isEditMode = false;
    // Resetear la variable de validación
    this.formSubmitted = false;
    // Mostrar el modal
    this.isDialogVisible = true;
  }

  closeDialog(): void {
    this.isDialogVisible = false;
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

   loadClient(client: any): void {
    this.isEditMode = true;
    this.clientForm.patchValue(client);
  }

  saveClient(): void {
    console.log('Método saveClient invocado');

    // Establecer formSubmitted a true para mostrar todas las validaciones
    this.formSubmitted = true;

    // Forzar la validación de todos los campos marcándolos como tocados
    Object.keys(this.clientForm.controls).forEach(field => {
      const control = this.clientForm.get(field);
      control?.markAsTouched({ onlySelf: true });
    });

    console.log('Estado del formulario:', this.clientForm.valid ? 'Válido' : 'Inválido');

    if (this.clientForm.invalid) {
      console.log('Formulario inválido. No se puede guardar.');
      return;
    }

    console.log('Formulario válido. Guardando...');
    const client = this.clientForm.value;
    /*const action = this.isEditMode
      ? this.clienteService.updateCliente(client)
      : this.clienteService.createClient(client);*/

    /*action.subscribe({
      next: () => {
        this.formSubmitted = false;
        this.clientForm.reset();
        this.isEditMode = false;
        this.isDialogVisible = false;
      },
      error: (err) => console.error('Error:', err)
    });*/

    // Como el código de guardar está comentado, añadimos esto para simular un guardado exitoso
    this.formSubmitted = false;
    this.clientForm.reset();
    this.isEditMode = false;
    this.isDialogVisible = false;
  }

  // Método auxiliar para obtener todos los errores del formulario
  getFormValidationErrors(): any {
    const errors: any = {};
    Object.keys(this.clientForm.controls).forEach(key => {
      const control = this.clientForm.get(key);
      if (control && control.errors) {
        errors[key] = control.errors;
      }
    });
    return errors;
  }

  cancelForm(): void {
    this.formSubmitted = false; // Resetear la variable
    this.clientForm.reset();
    this.isEditMode = false;
    this.isDialogVisible = false; // Cierra el modal
  }

  editClient(client: any): void {
    console.log('Editando cliente:', client);

    // Limpiar el formulario antes de cargar los datos
    this.clientForm.reset();

    // Resetear la variable de validación
    this.formSubmitted = false;

    // Mostrar el modal
    this.isDialogVisible = true;

    // Pequeño retraso para asegurar que el modal se haya abierto completamente
    setTimeout(() => {
      // Ajustar los datos para el formulario (mapear campos si es necesario)
      const clientFormData = {
        clienteid: client.id,
        nombre: client.nombre,
        genero: client.genero,
        edad: client.edad,
        identificacion: client.identificacion,
        direccion: client.direccion,
        telefono: client.telefono,
        contrasena: client.contrasena || '', // Si no tiene contraseña, usar cadena vacía
        estado: client.estado
      };

      // Cargar los datos en el formulario
      this.loadClient(clientFormData);
    }, 100);
  }

  // Variables para el modal de confirmación de eliminación
  isDeleteModalVisible = false;
  clientToDelete: any = null;

  // Método para mostrar el modal de confirmación de eliminación
  confirmDelete(client: any): void {
    this.clientToDelete = client;
    this.isDeleteModalVisible = true;
  }

  // Método para cancelar la eliminación
  cancelDelete(): void {
    this.clientToDelete = null;
    this.isDeleteModalVisible = false;
  }

  // Método para eliminar el cliente
  deleteClient(): void {
    if (this.clientToDelete) {
      console.log('Eliminando cliente:', this.clientToDelete);

      // Aquí iría el código para eliminar el cliente a través del servicio
      /*this.clienteService.deleteCliente(this.clientToDelete.id).subscribe({
        next: () => {
          console.log('Cliente eliminado con éxito');
          // Actualizar la lista de clientes
          this.cargarCuentas();
          // Cerrar el modal de confirmación
          this.cancelDelete();
        },
        error: (err) => {
          console.error('Error al eliminar cliente:', err);
          // Aquí podrías mostrar un mensaje de error
        }
      });*/

      // Como el código para eliminar está comentado, simulamos la eliminación
      // Eliminamos el cliente de la lista local
      this.filteredItems = this.filteredItems.filter(item => item.id !== this.clientToDelete.id);
      this.clientes = this.clientes.filter(item => item.id !== this.clientToDelete.id);

      // Actualizamos la paginación y la vista
      this.updatePagination();
      this.updatePage();

      // Cerramos el modal de confirmación
      this.cancelDelete();
    }
  }
}
