import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Player, PlayerRequest } from '../models/player.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {
  private readonly apiService = inject(ApiService);

  getPlayers(): Observable<Player[]> {
    return this.apiService.get<Player[]>('Player');
  }

  createPlayer(request: PlayerRequest): Observable<Player> {
    return this.apiService.post<Player>('Player', request);
  }

  updatePlayer(id: number, request: PlayerRequest): Observable<void> {
    return this.apiService.put<void>(`Player/${id}`, request);
  }

  deletePlayer(id: number): Observable<void> {
    return this.apiService.delete<void>(`Player/${id}`);
  }
}
