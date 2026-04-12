# EventsManagement API

A **.NET 10 ASP.NET Core Web API** for managing community events, built with **Domain-Driven Design (DDD)** and **CQRS** principles.

---

## Table of Contents

- [EventsManagement API](#eventsmanagement-api)
  - [Table of Contents](#table-of-contents)
  - [Architecture Overview](#architecture-overview)
  - [Solution Structure](#solution-structure)
    - [Project Responsibilities](#project-responsibilities)
  - [Domain Model](#domain-model)
    - [Aggregate Root](#aggregate-root)
    - [Value Objects (Records)](#value-objects-records)
    - [Domain ID](#domain-id)
    - [Commands](#commands)
    - [Domain Events](#domain-events)
  - [API Endpoints](#api-endpoints)
    - [Events](#events)
      - [`POST /v1/events/`](#post-v1events)
    - [OpenAPI / Scalar](#openapi--scalar)
  - [Infrastructure](#infrastructure)
    - [Database](#database)
    - [Repository](#repository)
    - [Logging](#logging)
    - [Observability](#observability)
  - [Configuration](#configuration)
    - [`appsettings.json`](#appsettingsjson)
    - [`appsettings.Development.json`](#appsettingsdevelopmentjson)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Run Locally](#run-locally)
    - [Database Migrations](#database-migrations)
  - [Patterns \& Libraries](#patterns--libraries)
  - [Implementation Status](#implementation-status)

---

## Architecture Overview

The solution uses a **modular, layered architecture** inspired by DDD and CQRS:

- **Write side**: HTTP requests are received by Minimal API endpoints, delegated to a Facade, translated into Commands, and dispatched to Command Handlers that operate on Aggregate Roots, persisting state via a Repository abstraction backed by EF Core.
- **Read side**: A separate `ReadModel` project is scaffolded as a placeholder for future CQRS query handling.
- **Modular composition root**: Each concern (logging, CORS, infrastructure, features) is encapsulated in an `IModule` implementation, registered and configured in order at startup.

```
HTTP Request
    │
    ▼
Minimal API Endpoint (EventsManagement.Events.Facade / Endpoints)
    │
    ▼
IEventsFacade (EventsFacade)
    │
    ▼
Command (CreateCommunityEvent) → ICommandHandlerAsync<TCommand>
    │
    ▼
CreateCommunityEventCommandHandler (EventsManagement.Events.Domain)
    │
    ▼
CommunityEvent Aggregate Root (EventsManagement.Events.Entities)
    │
    ▼
IEventsManagementRepository → CommunityEventsRepository (EF Core / SQL Server)
```

---

## Solution Structure

```
EventsManagement.slnx
├── EventsManagement.Rest                   # ASP.NET Core host — Minimal APIs, modules, startup
├── EventsManagement.Shared                 # Cross-cutting abstractions: base types, contracts, validators
│
├── Events/
│   ├── EventsManagement.Events.SharedKernel   # Value objects, Commands, Domain Events
│   ├── EventsManagement.Events.Entities       # Aggregate root (CommunityEvent), persistence DTOs
│   ├── EventsManagement.Events.Domain         # Command handlers (application/domain logic)
│   ├── EventsManagement.Events.Infrastructure # EF Core DbContext, repository implementation
│   ├── EventsManagement.Events.Facade         # Facade service + Minimal API endpoint registration
│   └── EventsManagement.Events.ReadModel      # Placeholder for future read/query model
│
├── Attendees/
│   └── EventsManagement.Attendees.Facade      # Stub module — no endpoints yet
│
└── EventsManagement.Infrastructure            # Cross-module infrastructure bootstrap
```

### Project Responsibilities

| Project | Layer | Responsibility |
|---|---|---|
| `EventsManagement.Rest` | Presentation | Host, module wiring, OpenAPI/Scalar, Serilog, CORS |
| `EventsManagement.Shared` | Shared | `DtoBase`, `CommunityEventId`, repository interface, query marker, base command handler, external contracts, `ValidationFilter<T>` |
| `EventsManagement.Events.SharedKernel` | Shared Kernel | Value objects (`EventName`, `EventVenue`, etc.), command and domain event message definitions |
| `EventsManagement.Events.Entities` | Domain Model | `CommunityEvent` aggregate root, persistence DTOs (`CommunityEventDto`, `OrganizersDto`), mapping helpers |
| `EventsManagement.Events.Domain` | Application / Domain | `CreateCommunityEventCommandHandler` |
| `EventsManagement.Events.Infrastructure` | Infrastructure | `EventsManagementContext` (EF Core), `CommunityEventsRepository`, fluent mappings |
| `EventsManagement.Events.Facade` | Application | `IEventsFacade` / `EventsFacade`, endpoint registration |
| `EventsManagement.Events.ReadModel` | Application (Read) | *(Placeholder — not yet implemented)* |
| `EventsManagement.Attendees.Facade` | Application | *(Stub — not yet implemented)* |
| `EventsManagement.Infrastructure` | Infrastructure | Cross-module infrastructure bootstrap (reads SQL connection string) |

---

## Domain Model

### Aggregate Root

**`CommunityEvent`** (inherits `Muflone.Core.AggregateRoot`)

| Property | Type | Description |
|---|---|---|
| `EventId` | `CommunityEventId` | Strongly-typed domain identifier (UUID v7) |
| `EventName` | `EventName` | Name of the event |
| `EventDescription` | `EventDescription` | Description of the event |
| `EventVenue` | `EventVenue` | Venue where the event takes place |
| `EventOrganizers` | `IEnumerable<EventOrganizer>` | List of organizer names |
| `EventDate` | `EventDate` | Date/time of the event |

Created via the static factory method `CommunityEvent.Create(...)`, which emits a `CommunityEventCreated` domain event.

### Value Objects (Records)

All defined in `EventsManagement.Events.SharedKernel.CustomTypes`:

- `EventName(string Value)`
- `EventDescription(string Value)`
- `EventVenue(string Value)`
- `EventOrganizer(string Value)`
- `EventDate(DateTime Value)`

### Domain ID

**`CommunityEventId`** — sealed class inheriting `Muflone.Core.DomainId`. New IDs are generated as `Guid.CreateVersion7()`.

### Commands

| Command | Payload |
|---|---|
| `CreateCommunityEvent` | `EventName`, `EventDescription`, `EventVenue`, `EventOrganizers`, `EventDate`, `CorrelationId` |

### Domain Events

| Event | Payload |
|---|---|
| `CommunityEventCreated` | same as `CreateCommunityEvent` |

---

## API Endpoints

Base URL: `http://localhost:5217` (HTTP) / `https://localhost:7100` (HTTPS)

### Events

| Method | Route | Description |
|---|---|---|
| `POST` | `/v1/events/` | Create a new community event |

#### `POST /v1/events/`

**Request body** (`application/json`):

```json
{
  "eventName": "string",
  "description": "string",
  "date": "2026-04-12T10:00:00",
  "venue": "string",
  "organizers": ["string"]
}
```

**Responses**:

| Status | Description |
|---|---|
| `201 Created` | Event created. `Location` header set to `/v1/events/{id}`. Body contains the new event ID. |
| `400 Bad Request` | Validation failure or domain error. |
| `500 Internal Server Error` | Unexpected server error. |

Request bodies are validated by `ValidationFilter<CreateCommunityEventJson>` (DataAnnotations, recursive).

### OpenAPI / Scalar

| Route | Description |
|---|---|
| `GET /openapi/v1.json` | OpenAPI specification (JSON) |
| `GET /scalar/v1` | Scalar API reference UI |

---

## Infrastructure

### Database

- **Provider**: Azure SQL Server — `Microsoft.EntityFrameworkCore.SqlServer` (EF Core 10)
- **DbContext**: `EventsManagementContext`
- **Tables**:
  - `dbo.CommunityEvents` — `Id` (PK, 36 chars), `EventName` (max 100), `EventDescription` (max 200), `EventVenue` (max 100), `EventDate`
  - `dbo.Organizers` — `Id` (PK), `EventId` (FK → `CommunityEvents.Id`, cascade delete), `OrganizerName` (max 100)

### Repository

`CommunityEventsRepository` implements `IEventsManagementRepository<CommunityEvent>`:

| Method | Status |
|---|---|
| `AddAsync` | ✅ Implemented — maps aggregate → DTO, uses a DB transaction, saves via EF Core |
| `GetByIdAsync` | ⬜ Not yet implemented |
| `UpdateAsync` | ⬜ Not yet implemented |
| `DeleteAsync` | ⬜ Not yet implemented |

### Logging

**Serilog** — configured from `appsettings.json`. Sinks: Console (structured), File.

### Observability

**OpenTelemetry** packages are referenced with instrumentation for ASP.NET Core, HTTP client, SQL Client, and Runtime. OTLP and Azure Monitor exporters are included. Full exporter configuration is not yet wired in code.

---

## Configuration

### `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### `appsettings.Development.json`

Contains development-environment overrides under the `EventsManagement` key:

```
EventsManagement:SqlServer:ConnectionString   — Azure SQL Server connection string
EventsManagement:SqlServer:SnapshotSize       — Snapshot page size (default: 1000)
EventsManagement:EventHub:EventHubConnectionString   — Azure Event Hub connection (planned)
EventsManagement:EventHub:EventHubName               — Event Hub name (planned)
EventsManagement:EventHub:BlobStorageConnectionString — Blob Storage for checkpointing (planned)
EventsManagement:EventHub:BlobStorageContainerName    — Blob container name (planned)
```

> **Security note**: `appsettings.Development.json` must not be committed with real credentials. Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) locally and Azure Key Vault in production.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- An Azure SQL Server database (or a local SQL Server / LocalDB instance with the connection string updated)

### Run Locally

```bash
# Restore dependencies
dotnet restore EventsManagement.slnx

# Build the solution
dotnet build EventsManagement.slnx

# Configure the connection string (use User Secrets to avoid committing credentials)
dotnet user-secrets set "EventsManagement:SqlServer:ConnectionString" "<your-connection-string>" \
  --project EventsManagement.Rest

# Run the API
dotnet run --project EventsManagement.Rest
```

The API will be available at `http://localhost:5217`.  
The Scalar UI will be at `http://localhost:5217/scalar/v1`.

### Database Migrations

No migration files are currently included. To create and apply the initial migration:

```bash
dotnet ef migrations add InitialCreate \
  --project Events/EventsManagement.Events.Infrastructure \
  --startup-project EventsManagement.Rest

dotnet ef database update \
  --project Events/EventsManagement.Events.Infrastructure \
  --startup-project EventsManagement.Rest
```

---

## Patterns & Libraries

| Pattern / Library | Details |
|---|---|
| **DDD** | Aggregate Root, Value Objects (C# records), Strongly-typed Domain IDs, Domain Events |
| **CQRS** | Commands → Command Handlers; separate ReadModel project for future query side |
| **Repository Pattern** | `IEventsManagementRepository<T>` abstraction over EF Core |
| **Facade Pattern** | `IEventsFacade` insulates the HTTP layer from domain internals |
| **Modular Composition Root** | Custom `IModule` interface; ordered, conditional module registration at startup |
| **Minimal API** | `MapGroup` + `MapPost` with endpoint filters — no MVC Controllers |
| **Result Monad** | [Lena](https://github.com/BrewUp/Lena) `Result<T>` with `.Match()` to avoid exception-driven flow |
| **Muflone** | DDD/CQRS framework: `AggregateRoot`, `DomainId`, `Command`, `DomainEvent`, `ICommandHandlerAsync<T>` |
| **EF Core 10** | SQL Server provider, Fluent API mappings, design-time tools |
| **Serilog** | Structured logging with console and file sinks |
| **OpenTelemetry** | ASP.NET Core, HTTP, SQL Client, and Runtime instrumentation; OTLP and Azure Monitor exporters |
| **Scalar / OpenAPI** | Modern API reference UI via `Scalar.AspNetCore` |
| **CORS** | `BrewUpCorsPolicy` — any origin, method, and header (development-friendly) |

---

## Implementation Status

| Feature | Status |
|---|---|
| Create Community Event (`POST /v1/events/`) | ✅ Full flow: HTTP → Facade → Command → Handler → EF Core → SQL Server |
| Attendees module | ⬜ Stub — no facade, no endpoints |
| Read / Query model | ⬜ Empty project placeholder |
| `GetById` / `Update` / `Delete` in repository | ⬜ `NotImplementedException` |
| Azure Event Hub publishing | ⬜ Config present, no publishing code |
| JWT Authentication | ⬜ Package referenced, middleware not wired |
| OpenTelemetry exporter configuration | ⬜ Packages referenced, not configured in code |
