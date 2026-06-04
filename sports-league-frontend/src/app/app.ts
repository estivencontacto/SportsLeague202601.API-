import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterLinkActive, RouterOutlet, MatButtonModule, MatToolbarModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly links = [
    { path: '/teams', label: 'Equipos' },
    { path: '/players', label: 'Jugadores' },
    { path: '/tournaments', label: 'Torneos' },
    { path: '/matches', label: 'Partidos' },
    { path: '/lineups', label: 'Alineaciones' },
    { path: '/referees', label: 'Arbitros' },
    { path: '/standings', label: 'Posiciones' }
  ];
}
