import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Referee, RefereeRequest } from '../models/referee.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class RefereeService {
  private readonly apiService = inject(ApiService);

  getReferees(): Observable<Referee[]> {
    return this.apiService.get<Referee[]>('Referee');
  }

  createReferee(request: RefereeRequest): Observable<Referee> {
    return this.apiService.post<Referee>('Referee', request);
  }

  updateReferee(id: number, request: RefereeRequest): Observable<void> {
    return this.apiService.put<void>(`Referee/${id}`, request);
  }

  deleteReferee(id: number): Observable<void> {
    return this.apiService.delete<void>(`Referee/${id}`);
  }
}
