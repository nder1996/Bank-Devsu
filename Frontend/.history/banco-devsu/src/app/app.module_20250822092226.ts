import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './presentation/layout/header/header.component';
import { SidebarComponent } from './presentation/layout/sidebar/sidebar.component';
import { ClientesComponent } from './presentation/pages/clientes/clientes.component';
import { CuentasComponent } from './presentation/pages/cuentas/cuentas.component';
import { MovimientosComponent } from './presentation/pages/movimientos/movimientos.component';
import { ReportesComponent } from './presentation/pages/reportes/reportes.component';
import { FooterComponent } from './presentation/layout/footer/footer.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    SidebarComponent,
    ClientesComponent,
    CuentasComponent,
    MovimientosComponent,
    ReportesComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule, 

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
