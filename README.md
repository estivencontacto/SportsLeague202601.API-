# SportsLeague API

Backend REST para gestionar una liga deportiva: equipos, jugadores, arbitros,
torneos, inscripciones, partidos, resultados, eventos de partido, alineaciones
y tabla de posiciones.

## Estado del proyecto

- Solucion .NET organizada en tres proyectos:
  - `SportsLeague.API`: controllers, DTOs, AutoMapper, Swagger y configuracion HTTP.
  - `SportsLeague.Domain`: entidades, enums, contratos y servicios de negocio.
  - `SportsLeague.DataAccess`: `DbContext`, repositorios, migraciones y seeders.
- Swagger disponible en `http://localhost:5105/swagger/index.html`.
- `ApplyMigrationsOnStartup` esta desactivado por defecto para permitir abrir
  Swagger sin depender de SQL Server.
- Para probar endpoints reales se requiere SQL Server Express, cadena de conexion
  valida y `Database:ApplyMigrationsOnStartup` en `true`.

## Funcionalidades

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

## Rubrica cubierta

### Fase 3

- `Referee`
- `Tournament`
- `TournamentTeam`
- Inscripcion de equipos
- Maquina de estados de torneo

### Fase 4

Endpoints de alineacion:

- `POST /api/match/{matchId}/lineup`
- `GET /api/match/{matchId}/lineup`
- `GET /api/match/{matchId}/lineup/team/{teamId}`
- `DELETE /api/match/{matchId}/lineup/{id}`

## Ejecutar localmente

Restaurar dependencias y compilar:

```powershell
dotnet restore
dotnet build
```

Ejecutar la API:

```powershell
dotnet run --project SportsLeague.API
```

Abrir Swagger:

```text
http://localhost:5105/swagger/index.html
```

## Base de datos

La configuracion principal esta en `SportsLeague.API/appsettings.json`.

Para usar SQL Server Express:

1. Configurar `ConnectionStrings:DefaultConnection`.
2. Cambiar `Database:ApplyMigrationsOnStartup` a `true`.
3. Ejecutar la API para aplicar migraciones y seeders.

Ejemplo de configuracion:

```json
{
  "Database": {
    "ApplyMigrationsOnStartup": true
  }
}
```

## Endpoints principales

### Equipos y jugadores

- `GET /api/team`
- `POST /api/team`
- `GET /api/player`
- `POST /api/player`

### Torneos

- `GET /api/tournament`
- `POST /api/tournament`
- `PATCH /api/tournament/{id}/status`
- `POST /api/tournament/{id}/teams`
- `GET /api/tournament/{id}/teams`

### Partidos

- `GET /api/match/tournament/{tournamentId}`
- `GET /api/match/{id}`
- `POST /api/match`
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

- `GET /api/standing/tournament/{tournamentId}`

## Preparacion para frontend

El backend ya expone DTOs separados para request y response, CORS configurable y
Swagger para generar clientes o documentar contratos. Las siguientes mejoras
recomendadas para una version de portafolio son:

- Estandarizar errores con `ProblemDetails` para que el frontend maneje mensajes
  de forma consistente.
- Agregar validaciones con Data Annotations en DTOs de entrada.
- Separar el registro de dependencias en metodos de extension por capa.
- Agregar paginacion y busqueda en listados principales.
- Incorporar pruebas unitarias para reglas de estado y pruebas de integracion para
  endpoints criticos.
- Documentar flujos de demo: crear torneo, inscribir equipos, iniciar torneo,
  programar partido, registrar alineacion, finalizar partido y consultar tabla.
