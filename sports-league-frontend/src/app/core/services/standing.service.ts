import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Standing } from '../models/standings.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class StandingService {
  private readonly apiService = inject(ApiService);

  getStandings(tournamentId: number): Observable<Standing[]> {
    return this.apiService.get<Standing[]>(`standings?tournamentId=${tournamentId}`);
  }
}
