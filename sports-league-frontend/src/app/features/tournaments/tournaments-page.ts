import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { Tournament, TournamentRequest } from '../../core/models/tournament.model';
import { Team } from '../../core/models/team.model';
import { NotificationService } from '../../core/services/notification.service';
import { TeamService } from '../../core/services/team.service';
import { TournamentService } from '../../core/services/tournament.service';
import { MATERIAL_IMPORTS } from '../../shared/material.imports';

@Component({
  selector: 'app-tournaments-page',
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './tournaments-page.html',
  styleUrl: '../../shared/components/feature-page.scss'
})
export class TournamentsPage {
  private readonly fb = inject(FormBuilder);
  private readonly tournamentService = inject(TournamentService);
  private readonly teamService = inject(TeamService);
  private readonly notifications = inject(NotificationService);

  protected readonly tournaments = signal<Tournament[]>([]);
  protected readonly teams = signal<Team[]>([]);
  protected readonly tournamentTeams = signal<Team[]>([]);
  protected readonly selectedTournamentForTeams = signal<number | null>(null);
  protected readonly selectedId = signal<number | null>(null);
  protected readonly displayedColumns = ['name', 'season', 'dates', 'status', 'teamsCount', 'actions'];

  protected readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    season: ['', Validators.required],
    startDate: ['', Validators.required],
    endDate: ['', Validators.required]
  });

  protected readonly registerForm = this.fb.nonNullable.group({
    tournamentId: [0, [Validators.required, Validators.min(1)]],
    teamId: [0, [Validators.required, Validators.min(1)]]
  });

  constructor() {
    this.loadTournaments();
    this.loadTeams();
  }

  protected loadTournaments(): void {
    this.tournamentService.getTournaments().subscribe({
      next: (tournaments) => this.tournaments.set(tournaments),
      error: () => this.notifications.showError('No se pudieron cargar los torneos')
    });
  }

  protected loadTeams(): void {
    this.teamService.getTeams().subscribe({
      next: (teams) => this.teams.set(teams),
      error: () => this.notifications.showError('No se pudieron cargar los equipos')
    });
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request = this.form.getRawValue() as TournamentRequest;
    const id = this.selectedId();
    const operation: Observable<unknown> = id
      ? this.tournamentService.updateTournament(id, request)
      : this.tournamentService.createTournament(request);

    operation.subscribe({
      next: () => {
        this.notifications.showSuccess(id ? 'Torneo actualizado' : 'Torneo creado');
        this.resetForm();
        this.loadTournaments();
      },
      error: () => this.notifications.showError('No se pudo guardar el torneo')
    });
  }

  protected edit(tournament: Tournament): void {
    this.selectedId.set(tournament.id);
    this.form.patchValue({
      name: tournament.name,
      season: tournament.season,
      startDate: tournament.startDate.substring(0, 10),
      endDate: tournament.endDate.substring(0, 10)
    });
  }

  protected delete(tournament: Tournament): void {
    this.tournamentService.deleteTournament(tournament.id).subscribe({
      next: () => {
        this.notifications.showSuccess('Torneo eliminado');
        this.loadTournaments();
      },
      error: () => this.notifications.showError('No se pudo eliminar el torneo')
    });
  }

  protected registerTeam(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const { tournamentId, teamId } = this.registerForm.getRawValue();
    this.tournamentService.registerTeam(tournamentId, teamId).subscribe({
      next: () => {
        this.notifications.showSuccess('Equipo inscrito en el torneo');
        this.loadTournamentTeams(tournamentId);
        this.loadTournaments();
      },
      error: () => this.notifications.showError('No se pudo inscribir el equipo')
    });
  }

  protected loadTournamentTeams(tournamentId: number): void {
    if (!tournamentId) {
      this.tournamentTeams.set([]);
      return;
    }

    this.selectedTournamentForTeams.set(tournamentId);
    this.tournamentService.getTeamsByTournament(tournamentId).subscribe({
      next: (teams) => this.tournamentTeams.set(teams),
      error: () => this.notifications.showError('No se pudieron cargar los equipos del torneo')
    });
  }

  protected getStatusLabel(status: number): string {
    const labels = ['Pendiente', 'En progreso', 'Finalizado'];
    return labels[status] ?? 'Sin estado';
  }

  protected resetForm(): void {
    this.selectedId.set(null);
    this.form.reset();
  }
}
