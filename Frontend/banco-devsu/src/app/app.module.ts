import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './presentation/layout/header/header.component';
import { SidebarComponent } from './presentation/layout/sidebar/sidebar.component';
import { ClientesComponent } from './presentation/pages/clientes/clientes.component';
import { CuentasComponent } from './presentation/pages/cuentas/cuentas.component';
import { MovimientosComponent } from './presentation/pages/movimientos/movimientos.component';
import { ReportesComponent } from './presentation/pages/reportes/reportes.component';
import { FooterComponent } from './presentation/layout/footer/footer.component';
import { SharedModule } from './presentation/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoggingInterceptor } from './core/interceptors/logging.interceptor';

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
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoggingInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
