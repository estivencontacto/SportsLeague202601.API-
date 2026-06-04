import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { Referee, RefereeRequest } from '../../core/models/referee.model';
import { NotificationService } from '../../core/services/notification.service';
import { RefereeService } from '../../core/services/referee.service';
import { MATERIAL_IMPORTS } from '../../shared/material.imports';

@Component({
  selector: 'app-referees-page',
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './referees-page.html',
  styleUrl: '../../shared/components/feature-page.scss'
})
export class RefereesPage {
  private readonly fb = inject(FormBuilder);
  private readonly refereeService = inject(RefereeService);
  private readonly notifications = inject(NotificationService);

  protected readonly referees = signal<Referee[]>([]);
  protected readonly selectedId = signal<number | null>(null);
  protected readonly displayedColumns = ['firstName', 'lastName', 'nationality', 'actions'];

  protected readonly form = this.fb.nonNullable.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    nationality: ['', Validators.required]
  });

  constructor() {
    this.loadReferees();
  }

  protected loadReferees(): void {
    this.refereeService.getReferees().subscribe({
      next: (referees) => this.referees.set(referees),
      error: () => this.notifications.showError('No se pudieron cargar los arbitros')
    });
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request = this.form.getRawValue() as RefereeRequest;
    const id = this.selectedId();
    const operation: Observable<unknown> = id
      ? this.refereeService.updateReferee(id, request)
      : this.refereeService.createReferee(request);

    operation.subscribe({
      next: () => {
        this.notifications.showSuccess(id ? 'Arbitro actualizado' : 'Arbitro creado');
        this.resetForm();
        this.loadReferees();
      },
      error: () => this.notifications.showError('No se pudo guardar el arbitro')
    });
  }

  protected edit(referee: Referee): void {
    this.selectedId.set(referee.id);
    this.form.patchValue(referee);
  }

  protected delete(referee: Referee): void {
    this.refereeService.deleteReferee(referee.id).subscribe({
      next: () => {
        this.notifications.showSuccess('Arbitro eliminado');
        this.loadReferees();
      },
      error: () => this.notifications.showError('No se pudo eliminar el arbitro')
    });
  }

  protected resetForm(): void {
    this.selectedId.set(null);
    this.form.reset();
  }
}
