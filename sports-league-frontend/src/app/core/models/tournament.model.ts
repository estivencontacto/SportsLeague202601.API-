export interface Tournament {
  id: number;
  name: string;
  season: string;
  startDate: string;
  endDate: string;
  status: number;
  teamsCount: number;
  createdAt: string;
  updatedAt?: string | null;
}

export interface TournamentRequest {
  name: string;
  season: string;
  startDate: string;
  endDate: string;
}
