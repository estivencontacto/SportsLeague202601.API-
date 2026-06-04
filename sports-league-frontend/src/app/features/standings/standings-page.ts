import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { Standing } from '../../core/models/standings.model';
import { Tournament } from '../../core/models/tournament.model';
import { NotificationService } from '../../core/services/notification.service';
import { StandingService } from '../../core/services/standing.service';
import { TournamentService } from '../../core/services/tournament.service';
import { MATERIAL_IMPORTS } from '../../shared/material.imports';

@Component({
  selector: 'app-standings-page',
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './standings-page.html',
  styleUrl: '../../shared/components/feature-page.scss'
})
export class StandingsPage {
  private readonly fb = inject(FormBuilder);
  private readonly standingService = inject(StandingService);
  private readonly tournamentService = inject(TournamentService);
  private readonly notifications = inject(NotificationService);

  protected readonly tournaments = signal<Tournament[]>([]);
  protected readonly standings = signal<Standing[]>([]);
  protected readonly displayedColumns = [
    'position',
    'teamName',
    'matchesPlayed',
    'wins',
    'draws',
    'losses',
    'goalsFor',
    'goalsAgainst',
    'goalDifference',
    'points'
  ];

  protected readonly form = this.fb.nonNullable.group({
    tournamentId: [0, [Validators.required, Validators.min(1)]]
  });

  constructor() {
    this.loadTournaments();
  }

  protected loadTournaments(): void {
    this.tournamentService.getTournaments().subscribe({
      next: (tournaments) => this.tournaments.set(tournaments),
      error: () => this.notifications.showError('No se pudieron cargar los torneos')
    });
  }

  protected loadStandings(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const tournamentId = this.form.controls.tournamentId.value;
    this.standingService.getStandings(tournamentId).subscribe({
      next: (standings) => this.standings.set(standings),
      error: () => this.notifications.showError('No se pudo cargar la tabla de posiciones')
    });
  }
}
