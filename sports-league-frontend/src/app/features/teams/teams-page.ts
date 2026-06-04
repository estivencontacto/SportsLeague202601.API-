import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { Team, TeamRequest } from '../../core/models/team.model';
import { NotificationService } from '../../core/services/notification.service';
import { TeamService } from '../../core/services/team.service';
import { MATERIAL_IMPORTS } from '../../shared/material.imports';

@Component({
  selector: 'app-teams-page',
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './teams-page.html',
  styleUrl: '../../shared/components/feature-page.scss'
})
export class TeamsPage {
  private readonly fb = inject(FormBuilder);
  private readonly teamService = inject(TeamService);
  private readonly notifications = inject(NotificationService);

  protected readonly teams = signal<Team[]>([]);
  protected readonly selectedId = signal<number | null>(null);
  protected readonly isLoading = signal(false);
  protected readonly displayedColumns = ['name', 'city', 'stadium', 'foundedDate', 'actions'];

  protected readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    city: ['', Validators.required],
    stadium: ['', Validators.required],
    logoUrl: [''],
    foundedDate: ['', Validators.required]
  });

  constructor() {
    this.loadTeams();
  }

  protected loadTeams(): void {
    this.isLoading.set(true);
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams.set(teams);
        this.isLoading.set(false);
      },
      error: () => this.handleError('No se pudieron cargar los equipos')
    });
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request = this.form.getRawValue() as TeamRequest;
    const id = this.selectedId();
    const operation: Observable<unknown> = id
      ? this.teamService.updateTeam(id, request)
      : this.teamService.createTeam(request);

    operation.subscribe({
      next: () => {
        this.notifications.showSuccess(id ? 'Equipo actualizado' : 'Equipo creado');
        this.resetForm();
        this.loadTeams();
      },
      error: () => this.handleError('No se pudo guardar el equipo')
    });
  }

  protected edit(team: Team): void {
    this.selectedId.set(team.id);
    this.form.patchValue({
      name: team.name,
      city: team.city,
      stadium: team.stadium,
      logoUrl: team.logoUrl ?? '',
      foundedDate: team.foundedDate.substring(0, 10)
    });
  }

  protected delete(team: Team): void {
    this.teamService.deleteTeam(team.id).subscribe({
      next: () => {
        this.notifications.showSuccess('Equipo eliminado');
        this.loadTeams();
      },
      error: () => this.handleError('No se pudo eliminar el equipo')
    });
  }

  protected resetForm(): void {
    this.selectedId.set(null);
    this.form.reset();
  }

  private handleError(message: string): void {
    this.isLoading.set(false);
    this.notifications.showError(message);
  }
}
