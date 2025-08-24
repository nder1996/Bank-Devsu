import { Component } from '@angular/core';

@Component({
  selector: 'app-movimientos',
  templateUrl: './movimientos.component.html',
  styleUrls: ['./movimientos.component.css']
})
export class MovimientosComponent {
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
}
