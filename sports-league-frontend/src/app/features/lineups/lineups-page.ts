import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { Match, MatchLineup, MatchLineupRequest } from '../../core/models/match.model';
import { Player } from '../../core/models/player.model';
import { Tournament } from '../../core/models/tournament.model';
import { MatchLineupService } from '../../core/services/match-lineup.service';
import { MatchService } from '../../core/services/match.service';
import { NotificationService } from '../../core/services/notification.service';
import { PlayerService } from '../../core/services/player.service';
import { TournamentService } from '../../core/services/tournament.service';
import { MATERIAL_IMPORTS } from '../../shared/material.imports';

@Component({
  selector: 'app-lineups-page',
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './lineups-page.html',
  styleUrl: '../../shared/components/feature-page.scss'
})
export class LineupsPage {
  private readonly fb = inject(FormBuilder);
  private readonly tournamentService = inject(TournamentService);
  private readonly matchService = inject(MatchService);
  private readonly playerService = inject(PlayerService);
  private readonly lineupService = inject(MatchLineupService);
  private readonly notifications = inject(NotificationService);

  protected readonly tournaments = signal<Tournament[]>([]);
  protected readonly matches = signal<Match[]>([]);
  protected readonly players = signal<Player[]>([]);
  protected readonly lineup = signal<MatchLineup[]>([]);
  protected readonly selectedMatchId = signal<number | null>(null);
  protected readonly displayedColumns = ['playerName', 'teamName', 'position', 'type', 'actions'];
  protected readonly positions = ['ARQ', 'DEF', 'MED', 'DEL'];

  protected readonly form = this.fb.nonNullable.group({
    tournamentId: [0, [Validators.required, Validators.min(1)]],
    matchId: [0, [Validators.required, Validators.min(1)]],
    playerId: [0, [Validators.required, Validators.min(1)]],
    position: ['', Validators.required],
    isStarter: [true, Validators.required]
  });

  constructor() {
    this.loadInitialData();
  }

  protected loadInitialData(): void {
    this.tournamentService.getTournaments().subscribe({
      next: (tournaments) => this.tournaments.set(tournaments),
      error: () => this.notifications.showError('No se pudieron cargar los torneos')
    });

    this.playerService.getPlayers().subscribe({
      next: (players) => this.players.set(players),
      error: () => this.notifications.showError('No se pudieron cargar los jugadores')
    });
  }

  protected loadMatches(tournamentId: number): void {
    this.matches.set([]);
    this.lineup.set([]);
    this.selectedMatchId.set(null);
    this.form.patchValue({ matchId: 0, playerId: 0 });

    if (!tournamentId) return;

    this.matchService.getByTournament(tournamentId).subscribe({
      next: (matches) => this.matches.set(matches),
      error: () => this.notifications.showError('No se pudieron cargar los partidos')
    });
  }

  protected loadLineup(matchId: number): void {
    this.selectedMatchId.set(matchId || null);
    this.lineup.set([]);

    if (!matchId) return;

    this.lineupService.getLineup(matchId).subscribe({
      next: (lineup) => this.lineup.set(lineup),
      error: () => this.notifications.showError('No se pudo cargar la alineacion')
    });
  }

  protected addPlayer(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const request: MatchLineupRequest = {
      playerId: value.playerId,
      position: value.position,
      isStarter: value.isStarter
    };

    this.lineupService.addPlayer(value.matchId, request).subscribe({
      next: () => {
        this.notifications.showSuccess('Jugador agregado a la alineacion');
        this.form.patchValue({ playerId: 0, position: '', isStarter: true });
        this.loadLineup(value.matchId);
      },
      error: () => this.notifications.showError('No se pudo agregar el jugador')
    });
  }

  protected deleteLineup(row: MatchLineup): void {
    this.lineupService.deleteLineup(row.matchId, row.id).subscribe({
      next: () => {
        this.notifications.showSuccess('Jugador eliminado de la alineacion');
        this.loadLineup(row.matchId);
      },
      error: () => this.notifications.showError('No se pudo eliminar la alineacion')
    });
  }
}
