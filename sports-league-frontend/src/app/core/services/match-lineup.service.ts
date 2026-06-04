import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { MatchLineup, MatchLineupRequest } from '../models/match.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class MatchLineupService {
  private readonly apiService = inject(ApiService);

  getLineup(matchId: number): Observable<MatchLineup[]> {
    return this.apiService.get<MatchLineup[]>(`match/${matchId}/lineup`);
  }

  addPlayer(matchId: number, request: MatchLineupRequest): Observable<MatchLineup> {
    return this.apiService.post<MatchLineup>(`match/${matchId}/lineup`, request);
  }

  deleteLineup(matchId: number, lineupId: number): Observable<void> {
    return this.apiService.delete<void>(`match/${matchId}/lineup/${lineupId}`);
  }
}
