import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Team, TeamRequest } from '../models/team.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  private readonly apiService = inject(ApiService);

  getTeams(): Observable<Team[]> {
    return this.apiService.get<Team[]>('Team');
  }

  createTeam(request: TeamRequest): Observable<Team> {
    return this.apiService.post<Team>('Team', request);
  }

  updateTeam(id: number, request: TeamRequest): Observable<void> {
    return this.apiService.put<void>(`Team/${id}`, request);
  }

  deleteTeam(id: number): Observable<void> {
    return this.apiService.delete<void>(`Team/${id}`);
  }
}
