import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { Match, MatchRequest } from '../../core/models/match.model';
import { Referee } from '../../core/models/referee.model';
import { Team } from '../../core/models/team.model';
import { Tournament } from '../../core/models/tournament.model';
import { MatchService } from '../../core/services/match.service';
import { NotificationService } from '../../core/services/notification.service';
import { RefereeService } from '../../core/services/referee.service';
import { TeamService } from '../../core/services/team.service';
import { TournamentService } from '../../core/services/tournament.service';
import { MATERIAL_IMPORTS } from '../../shared/material.imports';

@Component({
  selector: 'app-matches-page',
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './matches-page.html',
  styleUrl: '../../shared/components/feature-page.scss'
})
export class MatchesPage {
  private readonly fb = inject(FormBuilder);
  private readonly matchService = inject(MatchService);
  private readonly tournamentService = inject(TournamentService);
  private readonly teamService = inject(TeamService);
  private readonly refereeService = inject(RefereeService);
  private readonly notifications = inject(NotificationService);

  protected readonly matches = signal<Match[]>([]);
  protected readonly tournaments = signal<Tournament[]>([]);
  protected readonly teams = signal<Team[]>([]);
  protected readonly referees = signal<Referee[]>([]);
  protected readonly selectedId = signal<number | null>(null);
  protected readonly selectedTournamentId = signal<number | null>(null);
  protected readonly displayedColumns = ['matchday', 'teams', 'date', 'venue', 'referee', 'status', 'actions'];

  protected readonly form = this.fb.nonNullable.group({
    tournamentId: [0, [Validators.required, Validators.min(1)]],
    homeTeamId: [0, [Validators.required, Validators.min(1)]],
    awayTeamId: [0, [Validators.required, Validators.min(1)]],
    refereeId: [0, [Validators.required, Validators.min(1)]],
    matchDate: ['', Validators.required],
    venue: ['', Validators.required],
    matchday: [1, [Validators.required, Validators.min(1)]]
  });

  constructor() {
    this.loadCatalogs();
  }

  protected loadCatalogs(): void {
    this.tournamentService.getTournaments().subscribe({
      next: (tournaments) => this.tournaments.set(tournaments),
      error: () => this.notifications.showError('No se pudieron cargar los torneos')
    });
    this.teamService.getTeams().subscribe({
      next: (teams) => this.teams.set(teams),
      error: () => this.notifications.showError('No se pudieron cargar los equipos')
    });
    this.refereeService.getReferees().subscribe({
      next: (referees) => this.referees.set(referees),
      error: () => this.notifications.showError('No se pudieron cargar los arbitros')
    });
  }

  protected loadMatches(tournamentId: number): void {
    if (!tournamentId) {
      this.matches.set([]);
      return;
    }

    this.selectedTournamentId.set(tournamentId);
    this.matchService.getByTournament(tournamentId).subscribe({
      next: (matches) => this.matches.set(matches),
      error: () => this.notifications.showError('No se pudieron cargar los partidos')
    });
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request = this.form.getRawValue() as MatchRequest;
    const id = this.selectedId();
    const operation: Observable<unknown> = id
      ? this.matchService.updateMatch(id, request)
      : this.matchService.createMatch(request);

    operation.subscribe({
      next: () => {
        this.notifications.showSuccess(id ? 'Partido actualizado' : 'Partido creado');
        this.resetForm();
        this.loadMatches(request.tournamentId);
      },
      error: () => this.notifications.showError('No se pudo guardar el partido')
    });
  }

  protected edit(match: Match): void {
    this.selectedId.set(match.id);
    this.form.patchValue({
      tournamentId: match.tournamentId,
      homeTeamId: match.homeTeamId,
      awayTeamId: match.awayTeamId,
      refereeId: match.refereeId,
      matchDate: match.matchDate.substring(0, 16),
      venue: match.venue,
      matchday: match.matchday
    });
  }

  protected delete(match: Match): void {
    this.matchService.deleteMatch(match.id).subscribe({
      next: () => {
        this.notifications.showSuccess('Partido eliminado');
        this.loadMatches(match.tournamentId);
      },
      error: () => this.notifications.showError('No se pudo eliminar el partido')
    });
  }

  protected getStatusLabel(status: number): string {
    const labels = ['Programado', 'En juego', 'Finalizado', 'Suspendido'];
    return labels[status] ?? 'Sin estado';
  }

  protected resetForm(): void {
    this.selectedId.set(null);
    this.form.reset({ tournamentId: this.selectedTournamentId() ?? 0, homeTeamId: 0, awayTeamId: 0, refereeId: 0, matchday: 1 });
  }
}
