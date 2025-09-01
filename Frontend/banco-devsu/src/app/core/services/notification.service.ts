import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Notification {
  type: 'success' | 'error' | 'warning';
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notification$ = new BehaviorSubject<Notification | null>(null);

  getNotification(): Observable<Notification | null> {
    return this.notification$.asObservable();
  }

  showSuccess(message: string): void {
    this.show('success', message);
  }

  showError(message: string): void {
    this.show('error', message);
  }

  showWarning(message: string): void {
    this.show('warning', message);
  }

  clear(): void {
    this.notification$.next(null);
  }

  private show(type: 'success' | 'error' | 'warning', message: string): void {
    this.notification$.next({ type, message });
    setTimeout(() => this.clear(), 4000);
  }
}
