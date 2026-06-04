import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Tournament, TournamentRequest } from '../models/tournament.model';
import { Team } from '../models/team.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class TournamentService {
  private readonly apiService = inject(ApiService);

  getTournaments(): Observable<Tournament[]> {
    return this.apiService.get<Tournament[]>('Tournament');
  }

  createTournament(request: TournamentRequest): Observable<Tournament> {
    return this.apiService.post<Tournament>('Tournament', request);
  }

  updateTournament(id: number, request: TournamentRequest): Observable<void> {
    return this.apiService.put<void>(`Tournament/${id}`, request);
  }

  deleteTournament(id: number): Observable<void> {
    return this.apiService.delete<void>(`Tournament/${id}`);
  }

  registerTeam(tournamentId: number, teamId: number): Observable<{ message: string }> {
    return this.apiService.post<{ message: string }>(`Tournament/${tournamentId}/teams`, { teamId });
  }

  getTeamsByTournament(tournamentId: number): Observable<Team[]> {
    return this.apiService.get<Team[]>(`Tournament/${tournamentId}/teams`);
  }
}
