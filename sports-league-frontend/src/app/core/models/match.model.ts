export interface Match {
  id: number;
  tournamentId: number;
  tournamentName: string;
  homeTeamId: number;
  homeTeamName: string;
  awayTeamId: number;
  awayTeamName: string;
  refereeId: number;
  refereeFullName: string;
  matchDate: string;
  venue: string;
  matchday: number;
  status: number;
  createdAt: string;
  updatedAt?: string | null;
}

export interface MatchRequest {
  tournamentId: number;
  homeTeamId: number;
  awayTeamId: number;
  refereeId: number;
  matchDate: string;
  venue: string;
  matchday: number;
}

export interface MatchLineup {
  id: number;
  matchId: number;
  playerId: number;
  playerName: string;
  teamName: string;
  isStarter: boolean;
  position: string;
}

export interface MatchLineupRequest {
  playerId: number;
  isStarter: boolean;
  position: string;
}
