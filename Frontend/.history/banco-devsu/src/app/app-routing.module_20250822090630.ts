import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/clientes', pathMatch: 'full' },
  { path: 'clientes', component: ClienteListComponent },
  { path: 'clientes/nuevo', component: ClienteFormComponent },
  { path: 'clientes/editar/:id', component: ClienteFormComponent },
  { path: 'cuentas', component: CuentaListComponent },
  //{ path: 'cuentas/nuevo', component: CuentaFormComponent },
  //{ path: 'cuentas/editar/:id', component: CuentaFormComponent },
  { path: 'movimientos', component: MovimientoListComponent },
  //{ path: 'movimientos/nuevo', component: MovimientoFormComponent },
  { path: 'reportes', component: ReporteComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
