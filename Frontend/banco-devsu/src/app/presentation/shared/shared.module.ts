import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastNotificationComponent } from './toast-notification/toast-notification.component';

@NgModule({
  declarations: [
    ToastNotificationComponent
  ],
  imports: [
    CommonModule,
    BrowserAnimationsModule
  ],
  exports: [
    ToastNotificationComponent
  ]
})
export class SharedModule { }
