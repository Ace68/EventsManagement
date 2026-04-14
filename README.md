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
    - [Events Aggregate Root](#events-aggregate-root)
    - [Notifications Aggregate Root](#notifications-aggregate-root)
    - [Value Objects (Records)](#value-objects-records)
    - [Domain ID](#domain-id)
    - [Commands](#commands)
    - [Queries](#queries)
  - [API Endpoints](#api-endpoints)
    - [Events](#events)
      - [`POST /v1/events/`](#post-v1events)
      - [`GET /v1/events/`](#get-v1events)
    - [Notifications](#notifications)
      - [`POST /v1/notifications/`](#post-v1notifications)
      - [`GET /v1/notifications/`](#get-v1notifications)
    - [OpenAPI / Scalar](#openapi--scalar)
  - [Infrastructure](#infrastructure)
    - [Database](#database)
    - [Repositories](#repositories)
    - [In-Memory Message Bus](#in-memory-message-bus)
    - [Azure Event Hub Listener](#azure-event-hub-listener)
    - [Logging](#logging)
    - [Observability](#observability)
  - [Configuration](#configuration)
    - [`appsettings.json`](#appsettingsjson)
    - [`appsettings.Development.json`](#appsettingsdevelopmentjson)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Run with Docker (SQL Server)](#run-with-docker-sql-server)
    - [Run Locally](#run-locally)
    - [Database Migrations](#database-migrations)
  - [Patterns \& Libraries](#patterns--libraries)
  - [Implementation Status](#implementation-status)

---

## Architecture Overview

The solution uses a **modular, layered architecture** inspired by DDD and CQRS:

- **Write side**: HTTP requests are received by Minimal API endpoints, delegated to a Facade, translated into Commands, and dispatched to Command Handlers that operate on Aggregate Roots, persisting state via a Repository abstraction backed by EF Core.
- **Read side**: Each module has a dedicated `ReadModel` project with implemented query handlers backed by EF Core.
- **In-process messaging**: An `InMemoryBroker` (`IServiceBus` / `IEventBus`) routes Commands and Events between modules without an external message broker.
- **External event ingestion**: The Notifications module includes a hosted service that consumes events from **Azure Event Hub** and dispatches Commands via the in-memory bus.
- **Modular composition root**: Each concern (logging, CORS, infrastructure, features) is encapsulated in an `IModule` implementation, registered and configured in order at startup.

```
HTTP Request
    │
    ▼
Minimal API Endpoint
    │
    ▼
IEventsFacade / INotificationsFacade
    │
    ▼
Command → ICommandHandlerAsync<TCommand> (Domain)
    │                         │
    ▼                         ▼ (Notifications only)
Aggregate Root          InMemoryBus (IServiceBus)
    │
    ▼
IEventsManagementRepository (EF Core / SQL Server)

Azure Event Hub ──► CommunityEventHubHandler ──► InMemoryBus ──► AddCommunityEventToScheduler
```

---

## Solution Structure

```
EventsManagement.slnx
├── EventsManagement.Rest                        # ASP.NET Core host — Minimal APIs, modules, startup
├── EventsManagement.Shared                      # Cross-cutting abstractions: base types, contracts, validators
├── EventsManagements.InMemoryBroker             # In-process IServiceBus / IEventBus implementation
│
├── Events/
│   ├── EventsManagement.Events.SharedKernel     # Value objects, Commands, Queries
│   ├── EventsManagement.Events.Entities         # CommunityEvent aggregate root, persistence DTOs
│   ├── EventsManagement.Events.Domain           # Command handlers
│   ├── EventsManagement.Events.Infrastructure   # EF Core DbContext, repository, fluent mappings
│   ├── EventsManagement.Events.Facade           # IEventsFacade / EventsFacade, endpoint registration
│   └── EventsManagement.Events.ReadModel        # GetCommunityEventsQueryHandler
│
├── Notifications/
│   ├── EventsManagement.Notifications.SharedKernel   # Value objects, Commands, Queries
│   ├── EventsManagement.Notifications.Entities       # EventsScheduler aggregate, persistence DTOs
│   ├── EventsManagement.Notifications.Domain         # AddCommunityEventCommandHandlerAsync
│   ├── EventsManagement.Notifications.Infrastructure # EF Core DbContext, repository, fluent mappings
│   ├── EventsManagement.Notifications.Facade         # INotificationsFacade / NotificationFacade, endpoint registration
│   └── EventsManagement.Notifications.ReadModel      # GetEventsSchedulerQueryHandler, EventHub listener hosted service
│
├── Attendees/
│   └── EventsManagement.Attendees.Facade        # Stub module — no endpoints yet
│
└── EventsManagement.Infrastructure              # Cross-module infrastructure bootstrap
```

### Project Responsibilities

| Project | Layer | Responsibility |
|---|---|---|
| `EventsManagement.Rest` | Presentation | Host, module wiring, OpenAPI/Scalar, Serilog, CORS |
| `EventsManagement.Shared` | Shared | `DtoBase`, `CommunityEventId`, repository/query interfaces, base handlers, external contracts, `ValidationFilter<T>`, `PagedResult<T>` |
| `EventsManagements.InMemoryBroker` | Infrastructure | `InMemoryBus` (`IServiceBus` + `IEventBus`), `InMemorySubscriber`, `InMemoryChannel` |
| `EventsManagement.Events.SharedKernel` | Shared Kernel | Value objects (`EventName`, `EventVenue`, etc.), `CreateCommunityEvent` command, `GetCommunityEvents` query |
| `EventsManagement.Events.Entities` | Domain Model | `CommunityEvent` aggregate root, `CommunityEventDto` / `OrganizersDto`, mapping helpers |
| `EventsManagement.Events.Domain` | Application / Domain | `CreateCommunityEventCommandHandler` |
| `EventsManagement.Events.Infrastructure` | Infrastructure | `EventsManagementContext` (EF Core), `CommunityEventsRepository`, fluent mappings |
| `EventsManagement.Events.Facade` | Application | `IEventsFacade` / `EventsFacade`, `EventsEndpoints` (POST + GET) |
| `EventsManagement.Events.ReadModel` | Application (Read) | `GetCommunityEventsQueryHandler` |
| `EventsManagement.Notifications.SharedKernel` | Shared Kernel | Value objects (`EventName`, `EventVenue`, `EventDate`), `AddCommunityEventToScheduler` command, `GetEventsScheduler` query |
| `EventsManagement.Notifications.Entities` | Domain Model | `EventsScheduler` aggregate, `EventsSchedulerDto`, mapping helpers |
| `EventsManagement.Notifications.Domain` | Application / Domain | `AddCommunityEventCommandHandlerAsync` |
| `EventsManagement.Notifications.Infrastructure` | Infrastructure | `NotificationsContext` (EF Core), `EventsSchedulerRepository`, fluent mappings |
| `EventsManagement.Notifications.Facade` | Application | `INotificationsFacade` / `NotificationFacade`, `NotificationsEndpoint` (POST + GET) |
| `EventsManagement.Notifications.ReadModel` | Application (Read) | `GetEventsSchedulerQueryHandler`, `CommunityEventHubHandler`, `EventHubListenerHostedService` |
| `EventsManagement.Attendees.Facade` | Application | *(Stub — not yet implemented)* |
| `EventsManagement.Infrastructure` | Infrastructure | Cross-module infrastructure bootstrap |

---

## Domain Model

### Events Aggregate Root

**`CommunityEvent`** (inherits `Muflone.Core.AggregateRoot`)

| Property | Type | Description |
|---|---|---|
| `EventId` | `CommunityEventId` | Strongly-typed domain identifier (UUID v7) |
| `EventName` | `EventName` | Name of the event |
| `EventDescription` | `EventDescription` | Description of the event |
| `EventVenue` | `EventVenue` | Venue where the event takes place |
| `EventOrganizers` | `IEnumerable<EventOrganizer>` | List of organizer names |
| `EventDate` | `EventDate` | Date/time of the event |

Created via the static factory method `CommunityEvent.Create(...)`.

### Notifications Aggregate Root

**`EventsScheduler`** — persists events added to the notification scheduler.

| Property | Type | Description |
|---|---|---|
| `EventId` | `CommunityEventId` | Reference to the scheduled community event |
| `EventName` | `EventName` | Name of the event |
| `EventVenue` | `EventVenue` | Venue of the event |
| `EventDate` | `EventDate` | Date/time of the event |

Created via `EventsScheduler.Create(...)`.

### Value Objects (Records)

**Events** (`EventsManagement.Events.SharedKernel.CustomTypes`):
- `EventName(string Value)`
- `EventDescription(string Value)`
- `EventVenue(string Value)`
- `EventOrganizer(string Value)`
- `EventDate(DateTime Value)`

**Notifications** (`EventsManagement.Notifications.SharedKernel.CustomTypes`):
- `EventName(string Value)`
- `EventVenue(string Value)`
- `EventDate(DateTime Value)`

### Domain ID

**`CommunityEventId`** — sealed class inheriting `Muflone.Core.DomainId`. New IDs are generated as `Guid.CreateVersion7()`.

### Commands

| Command | Module | Payload |
|---|---|---|
| `CreateCommunityEvent` | Events | `EventName`, `EventDescription`, `EventVenue`, `EventOrganizers`, `EventDate`, `CorrelationId` |
| `AddCommunityEventToScheduler` | Notifications | `CommunityEventId`, `EventName`, `EventVenue`, `EventDate` |

### Queries

| Query | Module | Parameters | Result |
|---|---|---|---|
| `GetCommunityEvents` | Events | `PageNumber`, `PageSize` | `PagedResult<CommunityEventDto>` |
| `GetEventsScheduler` | Notifications | `PageNumber`, `PageSize` | `PagedResult<EventsSchedulerDto>` |

---

## API Endpoints

Base URL: `http://localhost:5217` (HTTP) / `https://localhost:7100` (HTTPS)

### Events

| Method | Route | Description |
|---|---|---|
| `POST` | `/v1/events/` | Create a new community event |
| `GET` | `/v1/events/?page={n}&pageSize={n}` | Get a paginated list of community events |

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

#### `GET /v1/events/`

**Query parameters**: `page` (int), `pageSize` (int)

**Responses**:

| Status | Description |
|---|---|
| `200 OK` | Returns `PagedResult<CommunityEventJson>`. |
| `404 Not Found` | No events found. |

### Notifications

| Method | Route | Description |
|---|---|---|
| `POST` | `/v1/notifications/` | Add a community event to the scheduler |
| `GET` | `/v1/notifications/?page={n}&pageSize={n}` | Get a paginated list of scheduled events |

#### `POST /v1/notifications/`

**Request body** (`application/json`):

```json
{
  "eventId": "string",
  "eventName": "string",
  "eventVenue": "string",
  "eventDate": "2026-04-12T10:00:00"
}
```

Dispatches an `AddCommunityEventToScheduler` command via `InMemoryBus`. The command is handled by `AddCommunityEventCommandHandlerAsync`, which persists an `EventsScheduler` record via EF Core.

**Responses**:

| Status | Description |
|---|---|
| `200 OK` | Event added to scheduler. Body contains the event ID. |
| `404 Not Found` | Handler or resource not found. |

Request bodies are validated by `ValidationFilter<AddCommunityEventToSchedulerJson>`.

#### `GET /v1/notifications/`

**Query parameters**: `page` (int), `pageSize` (int)

**Responses**:

| Status | Description |
|---|---|
| `200 OK` | Returns `PagedResult<EventsSchedulerJson>`. |
| `404 Not Found` | No scheduled events found. |

### OpenAPI / Scalar

| Route | Description |
|---|---|
| `GET /openapi/v1.json` | OpenAPI specification (JSON) |
| `GET /scalar/v1` | Scalar API reference UI |

---

## Infrastructure

### Database

- **Provider**: SQL Server — `Microsoft.EntityFrameworkCore.SqlServer` (EF Core 10)
- **Local development**: SQL Server 2025 via Docker (`docker/docker-compose.yml`, port `1433`)
- **Cloud**: Azure SQL Server (configured via `appsettings.Development.json`)

**DbContexts and tables**:

| DbContext | Table | Columns |
|---|---|---|
| `EventsManagementContext` | `dbo.CommunityEvents` | `Id` (PK), `EventName`, `EventDescription`, `EventVenue`, `EventDate` |
| `EventsManagementContext` | `dbo.Organizers` | `Id` (PK), `EventId` (FK → `CommunityEvents`, cascade), `OrganizerName` |
| `NotificationsContext` | `dbo.EventsScheduler` | `Id` (PK), `EventName`, `EventVenue`, `EventDate` |

### Repositories

Both repositories implement `IEventsManagementRepository<T>` and skip DB transactions when the EF Core provider is InMemory (test support):

| Repository | `AddAsync` | `GetByIdAsync` | `UpdateAsync` | `DeleteAsync` |
|---|---|---|---|---|
| `CommunityEventsRepository` | ✅ | ⬜ | ⬜ | ⬜ |
| `EventsSchedulerRepository` | ✅ | ⬜ | ⬜ | ⬜ |

### In-Memory Message Bus

**`EventsManagements.InMemoryBroker`** provides an in-process message bus used by the Notifications module:

- `InMemoryBus` implements both `IServiceBus` (for Commands) and `IEventBus` (for Events).
- `InMemorySubscriber` maintains a registry of `IInMemoryChannel` subscribers keyed by message type.
- `InMemoryChannel<T>` wraps a `System.Threading.Channels.Channel<IMessage>` for non-blocking dispatch.
- Commands and events are written to the channel and consumed asynchronously by registered handlers.

### Azure Event Hub Listener

The `EventsManagement.Notifications.ReadModel` project hosts a `BackgroundService` (`EventHubListenerHostedService`) that wraps `CommunityEventHubHandler`:

- Uses `Azure.Messaging.EventHubs.Processor.EventProcessorClient` with Blob Storage checkpointing.
- Listens to the `Notifications` consumer group on the configured Event Hub.
- Parses incoming CDC-style JSON events and, when the source table is `[dbo].[CommunityEvents]`, dispatches an `AddCommunityEventToScheduler` command via `IServiceBus`.

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

Contains development-environment overrides under two top-level keys:

```
# Local Docker SQL Server
EventsManagementLocal:SqlServer:ConnectionString   — Local SQL Server connection string (Docker)
EventsManagementLocal:SqlServer:SnapshotSize       — Snapshot page size (default: 1000)

# Azure resources
EventsManagement:SqlServer:ConnectionString              — Azure SQL Server connection string
EventsManagement:SqlServer:SnapshotSize                  — Snapshot page size (default: 1000)
EventsManagement:EventHub:EventHubConnectionString       — Azure Event Hub connection string
EventsManagement:EventHub:EventHubName                   — Event Hub name
EventsManagement:EventHub:BlobStorageConnectionString    — Blob Storage connection string (checkpointing)
EventsManagement:EventHub:BlobStorageContainerName       — Blob container name
```

> **Security note**: `appsettings.Development.json` must **never** be committed with real credentials. Use [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) locally and **Azure Key Vault** in production.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for local SQL Server) **or** an Azure SQL Server instance

### Run with Docker (SQL Server)

```bash
# From the repository root — starts SQL Server 2025 on port 1433
cd docker
docker-compose up -d
```

The default SA password is `#brew2026Up` (override with the `Sa_Password` environment variable).

### Run Locally

```bash
# Restore and build
dotnet restore src/Api/EventsManagement.slnx
dotnet build src/Api/EventsManagement.slnx

# Set the local connection string via User Secrets (avoids committing credentials)
dotnet user-secrets set "EventsManagementLocal:SqlServer:ConnectionString" "<your-connection-string>" \
  --project src/Api/EventsManagement.Rest

# Run the API
dotnet run --project src/Api/EventsManagement.Rest
```

The API will be available at `http://localhost:5217`.  
The Scalar UI will be at `http://localhost:5217/scalar/v1`.

### Database Migrations

No migration files are currently included. To create and apply the initial migrations:

```bash
# Events schema
dotnet ef migrations add InitialCreate \
  --project src/Api/Events/EventsManagement.Events.Infrastructure \
  --startup-project src/Api/EventsManagement.Rest

dotnet ef database update \
  --project src/Api/Events/EventsManagement.Events.Infrastructure \
  --startup-project src/Api/EventsManagement.Rest

# Notifications schema
dotnet ef migrations add InitialCreate \
  --project src/Api/Notifications/EventsManagement.Notifications.Infrastructure \
  --startup-project src/Api/EventsManagement.Rest

dotnet ef database update \
  --project src/Api/Notifications/EventsManagement.Notifications.Infrastructure \
  --startup-project src/Api/EventsManagement.Rest
```

---

## Patterns & Libraries

| Pattern / Library | Details |
|---|---|
| **DDD** | Aggregate Root, Value Objects (C# records), Strongly-typed Domain IDs |
| **CQRS** | Commands → Command Handlers; Queries → Query Handlers (read model); separate write and read paths |
| **Repository Pattern** | `IEventsManagementRepository<T>` abstraction over EF Core |
| **Facade Pattern** | `IEventsFacade` / `INotificationsFacade` insulate the HTTP layer from domain internals |
| **Modular Composition Root** | Custom `IModule` interface; ordered, conditional module registration at startup |
| **Minimal API** | `MapGroup` + `MapPost`/`MapGet` with endpoint filters — no MVC Controllers |
| **Result Monad** | [Lena](https://github.com/BrewUp/Lena) `Result<T>` with `.Match()` to avoid exception-driven flow |
| **Muflone** | DDD/CQRS framework: `AggregateRoot`, `DomainId`, `Command`, `ICommandHandlerAsync<T>`, `IServiceBus`, `IEventBus` |
| **In-Memory Bus** | Custom `InMemoryBroker` — `Channel<T>`-based in-process command/event dispatch |
| **EF Core 10** | SQL Server provider, Fluent API mappings, design-time tools |
| **Azure Event Hubs** | `Azure.Messaging.EventHubs.Processor` — `EventProcessorClient` with Blob Storage checkpointing |
| **Serilog** | Structured logging with console and file sinks |
| **OpenTelemetry** | ASP.NET Core, HTTP, SQL Client, and Runtime instrumentation; OTLP and Azure Monitor exporters |
| **Scalar / OpenAPI** | Modern API reference UI via `Scalar.AspNetCore` |
| **CORS** | `BrewUpCorsPolicy` — any origin, method, and header (development-friendly) |
| **Docker** | SQL Server 2025 container for local development (`docker/docker-compose.yml`) |

---

## Implementation Status

| Feature | Status |
|---|---|
| Create Community Event (`POST /v1/events/`) | ✅ Full flow: HTTP → Facade → Command → Handler → EF Core → SQL Server |
| List Community Events (`GET /v1/events/`) | ✅ Full flow: HTTP → Facade → Query → QueryHandler → EF Core → SQL Server |
| Add Event to Scheduler (`POST /v1/notifications/`) | ✅ Full flow: HTTP → Facade → InMemoryBus → Command → Handler → EF Core → SQL Server |
| Get Scheduler (`GET /v1/notifications/`) | ✅ Full flow: HTTP → Facade → Query → QueryHandler → EF Core → SQL Server |
| Azure Event Hub → Scheduler (CDC ingestion) | ✅ `EventHubListenerHostedService` + `CommunityEventHubHandler` wired and registered |
| `GetById` / `Update` / `Delete` in repositories | ⬜ `NotImplementedException` |
| Attendees module | ⬜ Stub — no endpoints |
| JWT Authentication | ⬜ Package referenced, middleware not wired |
| OpenTelemetry exporter configuration | ⬜ Packages referenced, not configured in code |
