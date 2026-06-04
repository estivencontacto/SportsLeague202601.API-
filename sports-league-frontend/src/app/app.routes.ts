import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'teams',
    pathMatch: 'full'
  },
  {
    path: 'teams',
    loadComponent: () => import('./features/teams/teams-page').then((m) => m.TeamsPage)
  },
  {
    path: 'players',
    loadComponent: () => import('./features/players/players-page').then((m) => m.PlayersPage)
  },
  {
    path: 'tournaments',
    loadComponent: () => import('./features/tournaments/tournaments-page').then((m) => m.TournamentsPage)
  },
  {
    path: 'matches',
    loadComponent: () => import('./features/matches/matches-page').then((m) => m.MatchesPage)
  },
  {
    path: 'lineups',
    loadComponent: () => import('./features/lineups/lineups-page').then((m) => m.LineupsPage)
  },
  {
    path: 'referees',
    loadComponent: () => import('./features/referees/referees-page').then((m) => m.RefereesPage)
  },
  {
    path: 'standings',
    loadComponent: () => import('./features/standings/standings-page').then((m) => m.StandingsPage)
  },
  {
    path: '**',
    redirectTo: 'teams'
  }
];
