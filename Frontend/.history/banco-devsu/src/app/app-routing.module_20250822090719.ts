import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientesComponent } from './presentation/pages/clientes/clientes.component';
import { CuentasComponent } from './presentation/pages/cuentas/cuentas.component';
import { MovimientosComponent } from './presentation/pages/movimientos/movimientos.component';

const routes: Routes = [
  { path: '', redirectTo: '/clientes', pathMatch: 'full' },
  { path: 'clientes', component: ClientesComponent },
  //{ path: 'clientes/nuevo', component: ClienteFormComponent },
  //{ path: 'clientes/editar/:id', component: ClienteFormComponent },
  { path: 'cuentas', component: CuentasComponent },
  //{ path: 'cuentas/nuevo', component: CuentaFormComponent },
  //{ path: 'cuentas/editar/:id', component: CuentaFormComponent },
  { path: 'movimientos', component: MovimientosComponent },
  //{ path: 'movimientos/nuevo', component: MovimientoFormComponent },
  { path: 'reportes', component: ReportesComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
