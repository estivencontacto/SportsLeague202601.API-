export interface Player {
  id: number;
  firstName: string;
  lastName: string;
  birthDate: string;
  number: number;
  position: number;
  teamId: number;
  teamName: string;
  createdAt: string;
  updatedAt?: string | null;
}

export interface PlayerRequest {
  firstName: string;
  lastName: string;
  birthDate: string;
  number: number;
  position: number;
  teamId: number;
}
