import { Component, OnInit } from '@angular/core';
import { MovimientoService } from 'src/app/core/services/movimiento.service';
import { Movimiento } from 'src/app/core/models/Movimiento';
import { TipoMovimiento } from '../../../core/enums/tipo-movimiento.enum';
import { MovimientoDTO } from 'src/app/core/dtos/movimiento.dto';

@Component({
  selector: 'app-movimientos',
  templateUrl: './movimientos.component.html',
  styleUrls: ['./movimientos.component.css']
})
export class MovimientosComponent implements OnInit {
  // Datos quemados
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

  movimientos: Movimiento[] = [];

  // Variables de búsqueda y filtrado
  searchTerm: string = '';
  filteredItems: any[] = [];

  // Variables de paginación
  paginatedItems: any[] = [];
  currentPage = 1;
  pageSize = 5;
  totalPages = 0;
  pages: number[] = [];
  startIndex = 0;
  endIndex = 0;

  constructor(private movimientoService: MovimientoService) {}

  ngOnInit() {
    this.cargarMovimientos();
  }

  cargarMovimientos(): void {
    this.movimientoService.getMovimientos().subscribe(
      (response: any) => {
        if (response.success) {
          console.log("json movimientos :", JSON.stringify(response.data));
          this.movimientos = response.data.map((movimiento: MovimientoDTO) => ({
            id: movimiento.id,
            fecha: new Date(movimiento.fecha).toLocaleDateString('es-ES', {
              year: 'numeric',
              month: '2-digit',
              day: '2-digit'
            }),
            tipoMovimiento: movimiento.tipoMovimiento === 1 ? 'Crédito' : 'Débito',
            valor: movimiento.valor,
            saldo: movimiento.saldo,
            cuenta: `Cuenta ${movimiento.cuenta}` // Formatear cuenta como "Cuenta X"
          }));
          this.filteredItems = [...this.movimientos];
          this.updatePagination();
          this.updatePage();
        } else {
          console.error('Errores en la respuesta:', response.errors);
        }
      },
      (error) => {
        console.error('Error al cargar movimientos:', error);
      }
    );
  }

  // Filtrar elementos según término de búsqueda
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

    this.currentPage = 1; // Volver a primera página
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
}
