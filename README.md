# SportsLeague

SportsLeague es una aplicacion academica full stack para administrar una liga deportiva. El proyecto incluye una API REST en .NET, persistencia con Entity Framework Core y un frontend en Angular con Angular Material.

La version actual esta preparada para sustentar gestion de equipos, jugadores, arbitros, torneos, inscripciones, partidos, resultados, alineaciones y tabla de posiciones.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server Express
- AutoMapper
- Angular
- Angular Material
- TypeScript

## Estructura

```text
SportsLeague.API/             Controllers, DTOs, AutoMapper, Swagger y configuracion HTTP
SportsLeague.DataAccess/      DbContext, repositorios, migraciones y seeders
SportsLeague.Domain/          Entidades, enums, contratos y servicios de negocio
sports-league-frontend/       Frontend Angular
```

## Funcionalidades backend

- CRUD de equipos.
- CRUD de jugadores con relacion a equipo.
- CRUD de arbitros.
- Gestion de torneos.
- Inscripcion de equipos a torneos.
- Maquina de estados de torneos.
- Programacion y gestion de partidos.
- Maquina de estados de partidos.
- Registro de resultados.
- Registro de goles y tarjetas.
- Alineaciones por partido.
- Estadisticas y tabla de posiciones.
- Seed de datos iniciales.
- CORS configurable para consumo desde frontend.

## Funcionalidades frontend

- Equipos: crear, listar, editar y eliminar.
- Jugadores: crear, listar, editar y eliminar.
- Arbitros: crear, listar, editar y eliminar.
- Torneos: crear, listar, editar, eliminar e inscribir equipos.
- Partidos: crear, listar por torneo, editar y eliminar.
- Alineaciones: consultar, agregar jugador y eliminar jugador.
- Posiciones: consultar tabla por torneo.

## Rubrica cubierta

### Fase 3

- Entidad `Referee`.
- Entidad `Tournament`.
- Entidad `TournamentTeam`.
- Inscripcion de equipos a torneos.
- Maquina de estados de torneo.

### Fase 4

Endpoints de alineacion:

- `POST /api/match/{matchId}/lineup`
- `GET /api/match/{matchId}/lineup`
- `GET /api/match/{matchId}/lineup/team/{teamId}`
- `DELETE /api/match/{matchId}/lineup/{id}`

Frontend Angular:

- Componentes standalone.
- Servicios HTTP por modulo.
- Modelos TypeScript.
- Angular Material.
- Rutas por funcionalidad.

## Requisitos

- Visual Studio 2026 o compatible con .NET 10.
- SQL Server Express.
- SQL Server Management Studio.
- Git.
- Node incluido localmente en `.tools` para ejecutar Angular en este equipo.

## Configuracion SQL

La API usa la siguiente cadena de conexion en `SportsLeague.API/appsettings.Development.json`:

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ITMSportsLeagueDb;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true;Connection Timeout=5;"
```

En SQL Server Management Studio conectarse con:

```text
localhost\SQLEXPRESS
```

Tambien se puede probar:

```text
.\SQLEXPRESS
```

Si tu instancia tiene otro nombre, actualiza el valor `Server=` en `SportsLeague.API/appsettings.Development.json`.

## Crear base de datos

Desde la raiz del proyecto:

```powershell
dotnet ef database update --project SportsLeague.DataAccess --startup-project SportsLeague.API
```

Por defecto `ApplyMigrationsOnStartup` esta en `false` para permitir abrir Swagger aunque SQL Server no este listo. Si se desea ejecutar migraciones y seeders automaticamente al iniciar la API, cambiar temporalmente:

```json
"ApplyMigrationsOnStartup": true
```

Para trabajo normal se recomienda dejarlo en `false`.

## Ejecutar API

Desde Visual Studio:

1. Abrir `SportsLeague.slnx`.
2. Seleccionar `SportsLeague.API` como proyecto de inicio.
3. Ejecutar el perfil `http`.
4. Abrir Swagger:

```text
http://localhost:5105/swagger/index.html
```

Desde terminal:

```powershell
dotnet run --project SportsLeague.API --launch-profile http
```

## Ejecutar frontend

La carpeta del frontend esta en:

```text
sports-league-frontend
```

Desde la raiz del proyecto:

```powershell
cd sports-league-frontend
& "..\.tools\node-v24.16.0-win-x64\npm.cmd" start
```

Abrir:

```text
http://localhost:4200
```

El frontend consume la API desde:

```text
http://localhost:5105/api
```

## Endpoints principales

### Equipos y jugadores

- `GET /api/team`
- `POST /api/team`
- `PUT /api/team/{id}`
- `DELETE /api/team/{id}`
- `GET /api/player`
- `POST /api/player`
- `PUT /api/player/{id}`
- `DELETE /api/player/{id}`

### Torneos

- `GET /api/tournament`
- `POST /api/tournament`
- `PUT /api/tournament/{id}`
- `DELETE /api/tournament/{id}`
- `PATCH /api/tournament/{id}/status`
- `POST /api/tournament/{id}/teams`
- `GET /api/tournament/{id}/teams`

### Partidos

- `GET /api/match/tournament/{tournamentId}`
- `GET /api/match/{id}`
- `POST /api/match`
- `PUT /api/match/{id}`
- `DELETE /api/match/{id}`
- `PATCH /api/match/{id}/status`

### Eventos de partido

- `POST /api/match/{matchId}/result`
- `GET /api/match/{matchId}/result`
- `POST /api/match/{matchId}/goals`
- `GET /api/match/{matchId}/goals`
- `POST /api/match/{matchId}/cards`
- `GET /api/match/{matchId}/cards`

### Alineaciones

- `POST /api/match/{matchId}/lineup`
- `GET /api/match/{matchId}/lineup`
- `GET /api/match/{matchId}/lineup/team/{teamId}`
- `DELETE /api/match/{matchId}/lineup/{id}`

### Estadisticas

- `GET /api/standings?tournamentId={id}`
- `GET /api/stats/scorers?tournamentId={id}`
- `GET /api/stats/cards?tournamentId={id}`

## Pruebas recomendadas

Backend:

```powershell
dotnet build SportsLeague.slnx
```

Frontend:

```powershell
cd sports-league-frontend
& "..\.tools\node-v24.16.0-win-x64\npm.cmd" run build
& "..\.tools\node-v24.16.0-win-x64\npm.cmd" test -- --watch=false
```

## Flujo de prueba para sustentacion

1. Confirmar que SQL Server Express esta iniciado.
2. Ejecutar migraciones con `dotnet ef database update`.
3. Ejecutar la API y validar Swagger.
4. Probar `GET /api/Team` en Swagger.
5. Ejecutar Angular en `http://localhost:4200`.
6. Crear o editar equipos.
7. Crear arbitros y jugadores.
8. Crear un torneo.
9. Inscribir equipos al torneo.
10. Crear un partido usando torneo, equipos y arbitro.
11. Registrar alineaciones del partido.
12. Consultar posiciones por torneo.

## Notas de arquitectura

El backend mantiene separacion por capas: API, DataAccess y Domain. El frontend mantiene una estructura por `core`, `shared`, `features` y `layout`, usando componentes standalone y servicios para centralizar las llamadas HTTP.

El objetivo es conservar un codigo claro, educativo y sustentable, sin sobrecargar la solucion con patrones innecesarios para el alcance academico.
