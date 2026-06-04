import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { Player, PlayerRequest } from '../../core/models/player.model';
import { Team } from '../../core/models/team.model';
import { NotificationService } from '../../core/services/notification.service';
import { PlayerService } from '../../core/services/player.service';
import { TeamService } from '../../core/services/team.service';
import { MATERIAL_IMPORTS } from '../../shared/material.imports';

@Component({
  selector: 'app-players-page',
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_IMPORTS],
  templateUrl: './players-page.html',
  styleUrl: '../../shared/components/feature-page.scss'
})
export class PlayersPage {
  private readonly fb = inject(FormBuilder);
  private readonly playerService = inject(PlayerService);
  private readonly teamService = inject(TeamService);
  private readonly notifications = inject(NotificationService);

  protected readonly players = signal<Player[]>([]);
  protected readonly teams = signal<Team[]>([]);
  protected readonly selectedId = signal<number | null>(null);
  protected readonly displayedColumns = ['name', 'number', 'position', 'teamName', 'actions'];
  protected readonly positions = [
    { value: 0, label: 'Arquero' },
    { value: 1, label: 'Defensa' },
    { value: 2, label: 'Mediocampista' },
    { value: 3, label: 'Delantero' },
    { value: 4, label: 'Director tecnico' }
  ];

  protected readonly form = this.fb.nonNullable.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    birthDate: ['', Validators.required],
    number: [1, [Validators.required, Validators.min(1)]],
    position: [0, Validators.required],
    teamId: [0, [Validators.required, Validators.min(1)]]
  });

  constructor() {
    this.loadInitialData();
  }

  protected loadInitialData(): void {
    this.loadPlayers();
    this.teamService.getTeams().subscribe({
      next: (teams) => this.teams.set(teams),
      error: () => this.notifications.showError('No se pudieron cargar los equipos')
    });
  }

  protected loadPlayers(): void {
    this.playerService.getPlayers().subscribe({
      next: (players) => this.players.set(players),
      error: () => this.notifications.showError('No se pudieron cargar los jugadores')
    });
  }

  protected save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request = this.form.getRawValue() as PlayerRequest;
    const id = this.selectedId();
    const operation: Observable<unknown> = id
      ? this.playerService.updatePlayer(id, request)
      : this.playerService.createPlayer(request);

    operation.subscribe({
      next: () => {
        this.notifications.showSuccess(id ? 'Jugador actualizado' : 'Jugador creado');
        this.resetForm();
        this.loadPlayers();
      },
      error: () => this.notifications.showError('No se pudo guardar el jugador')
    });
  }

  protected edit(player: Player): void {
    this.selectedId.set(player.id);
    this.form.patchValue({
      firstName: player.firstName,
      lastName: player.lastName,
      birthDate: player.birthDate.substring(0, 10),
      number: player.number,
      position: player.position,
      teamId: player.teamId
    });
  }

  protected delete(player: Player): void {
    this.playerService.deletePlayer(player.id).subscribe({
      next: () => {
        this.notifications.showSuccess('Jugador eliminado');
        this.loadPlayers();
      },
      error: () => this.notifications.showError('No se pudo eliminar el jugador')
    });
  }

  protected getPositionLabel(position: number): string {
    return this.positions.find((item) => item.value === position)?.label ?? 'Sin definir';
  }

  protected resetForm(): void {
    this.selectedId.set(null);
    this.form.reset({ number: 1, position: 0, teamId: 0 });
  }
}
