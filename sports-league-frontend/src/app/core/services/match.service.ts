import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Match, MatchRequest } from '../models/match.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private readonly apiService = inject(ApiService);

  getByTournament(tournamentId: number): Observable<Match[]> {
    return this.apiService.get<Match[]>(`Match/tournament/${tournamentId}`);
  }

  createMatch(request: MatchRequest): Observable<Match> {
    return this.apiService.post<Match>('Match', request);
  }

  updateMatch(id: number, request: MatchRequest): Observable<void> {
    return this.apiService.put<void>(`Match/${id}`, request);
  }

  deleteMatch(id: number): Observable<void> {
    return this.apiService.delete<void>(`Match/${id}`);
  }
}
