export interface Team {
  id: number;
  name: string;
  city: string;
  stadium: string;
  logoUrl?: string | null;
  foundedDate: string;
  createdAt: string;
  updatedAt?: string | null;
}

export type TeamRequest = Pick<Team, 'name' | 'city' | 'stadium' | 'logoUrl' | 'foundedDate'>;
