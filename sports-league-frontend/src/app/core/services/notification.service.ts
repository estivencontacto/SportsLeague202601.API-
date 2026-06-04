import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly snackBar = inject(MatSnackBar);

  showSuccess(message: string): void {
    this.open(message, 'success-notification');
  }

  showError(message: string): void {
    this.open(message, 'error-notification');
  }

  private open(message: string, panelClass: string): void {
    this.snackBar.open(message, 'Cerrar', {
      duration: 3500,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass
    });
  }
}
