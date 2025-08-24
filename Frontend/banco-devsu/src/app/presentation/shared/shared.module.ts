import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { AlertsComponent } from './alerts/alerts.component';



@NgModule({
  declarations: [
    SearchBarComponent,
    AlertsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
