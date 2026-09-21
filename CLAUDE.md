# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository scope: this is one repo in a multi-repo solution

`ConnectLive.Core.sln` does not build from this repo alone. Several `ProjectReference`s point outside this
folder (`..\Workers`, `..\Connectlive.Proxy`, `..\Connectlive.WebHosting`, `..\ConnectLive.SPA`,
`..\ConnectLive.SPA.Infrastructure`, `..\WebHostExtensions`), expecting sibling repositories checked out next
to this one under the same parent folder (e.g. `repos\Workers`, `repos\Connectlive.Proxy`, ...). Docker builds
confirm this: each `Dockerfile`'s compose build `context` is `..` (the parent of this repo) and explicitly
`COPY`s csproj files from those sibling folders. If those siblings aren't present, restore/build fails on
missing project references — that's an environment issue, not a bug in this repo.

## Commands

- Restore/build the whole solution: `dotnet restore ConnectLive.Core.sln` / `dotnet build ConnectLive.Core.sln`
  (requires the sibling repos above to exist).
- Run one service directly: `dotnet run --project ConnectLive.Core\ConnectLive.Core.Api.csproj` (main API),
  `dotnet run --project ConnectLive.Notification.Api\ConnectLive.Notification.Api.csproj`, or
  `dotnet run --project ConnectLive.Newsletter.Api\ConnectLive.Newsletter.Api.csproj`.
- Run the full local stack (Postgres, RabbitMQ, Seq, Adminer + the 3 APIs) via Docker Compose:
  `docker compose -f docker-compose.yml -f docker-compose.override.yml up --build`. `docker-compose.qa.yml` /
  `docker-compose.debug.yml` are additional overlays for other environments.
- EF Core migrations (the `DbContext` lives in `ConntectLive.DAL`, DI/config live in `ConnectLive.Core.Api`):
  `dotnet ef migrations add <Name> --project ConntectLive.DAL --startup-project ConnectLive.Core\ConnectLive.Core.Api.csproj`,
  then `dotnet ef database update ...` the same way.
- There is no test project in the solution.

## Architecture

### Three thin API hosts over a shared kernel

The solution builds three separate ASP.NET Core 8 Web APIs, each a small host that wires up shared libraries
rather than owning business logic itself:

- **ConnectLive.Core** (`ConnectLive.Core.Api`) — the main API: users, questions, sessions, leaderboard
  endpoints; owns the MediatR/CQRS pipeline, publishes to the event bus, hosts the Hangfire dashboard at
  `/workers`.
- **ConnectLive.Notification.Api** — runs a Hangfire server pinned to the `"email"` queue and a MassTransit
  consumer (`UserCreatedConsumer`).
- **ConnectLive.Newsletter.Api** — runs a Hangfire server pinned to the `"newsletter"` queue.

Shared libraries in this repo:

- **ConnectLive.Domain** — plain models (`Model/`) and cross-service message contracts (`Contracts/`, e.g.
  `UserCreatedContractQuery`). No infrastructure dependencies; everything else depends on it.
- **ConntectLive.DAL** (name is intentionally spelled "Conntect" — keep it consistent, don't "fix" the typo) —
  EF Core + Npgsql data layer: `ApplicationDbContext`, `[Table]`/`[Column]`-attributed entities, `Migrations/`,
  and the generic repository/unit-of-work implementation.
- **ConnectLive.Application** — cross-cutting MediatR building blocks shared by the hosts: pipeline behaviors,
  the `IntegrationEvent` base type, bus consumers (`BusEvents/`), exception middleware.
- **ConnectLive.Portal.Shared** — the API's request/response DTOs (the wire contracts), separate from the
  MediatR commands/queries and from the EF entities.

Building blocks consumed from **sibling repos** (see scope note above): `Workers` (`IEmailWorker`, a Hangfire
job implementation), `Connectlive.Proxy` (`IProxy`, a typed HTTP client for calling other services),
`Connectlive.WebHosting` (`WebHostExtensions`).

### CQRS via MediatR, grouped by feature

Controllers are thin: they translate an HTTP call directly into `_mediator.Send(...)` and return the result
(see `UserController`). Requests and their handlers are grouped per feature into one static container class
under `Commands/` or `Queries/` — e.g. `UserCommands.SaveUserCommand` + `SaveUserCommandHandler`, or
`UserQueries.GetUserQuery`/`GetUsersQuery` + their handlers — with each command/query pair kept together in a
`#region` in the same file, rather than one class per file. Follow this grouping for new features.

Handlers resolve their dependencies from a single `IServiceProvider` passed into the constructor
(`GetRequiredService<T>()` calls in the body) instead of normal constructor-injected parameters — this is the
established pattern here (see `SaveUserCommandHandler`, `GetUsersQueryHandler`); match it for new handlers.

### Cross-cutting pipeline behaviors (`ConnectLive.Application`)

Two open-generic `IPipelineBehavior<,>` are registered globally for every MediatR request in `Program.cs`:

- `WatchBehavior<,>` — logs start/end and elapsed ms for every command/query. If the request derives from
  `IntegrationEvent`, its `EventId` is used to correlate the log lines.
- `CacheBehavior<,>` — opt-in response caching backed by `IMemoryCache` with sliding expiration. A request
  only gets cached if it implements `ICachable<TResponse>` (`Key` + `Expiration` in seconds) and
  `Expiration > 0`; anything else passes straight through.

New commands/queries that should be traceable inherit `IntegrationEvent`; new queries that should be cached
implement `ICachable<TResponse>` (see `UserQueries.GetUserQuery`/`GetUsersQuery`).

### Data access: generic repository + unit of work

`IGenericRepository<TEntity>` is a single EF Core-backed CRUD implementation registered as an open generic;
`IUnitOfWork` exposes one repository property per aggregate (`Users`, `Questions`) plus `Commit()` (a single
`SaveChanges()`). Adding a new persisted entity means: an entity class in `ConntectLive.DAL/Entity`
implementing `IEntity`, a repository property added to `IUnitOfWork`/`UnitOfWork`, and a `DbSet<>` on
`ApplicationDbContext`. A second, unrelated `IUnitOfWork<C>`/`UnitOfWork<C>` (generic over any `DbContext`) also
exists for cases that don't fit the `Users`/`Questions`-specific interface.

### Messaging and background jobs

- **MassTransit + RabbitMQ** carries pub/sub integration events between services. Contracts live in
  `ConnectLive.Domain/Contracts`, consumers in `ConnectLive.Application/BusEvents` (e.g.
  `UserCreatedConsumer`). Bus connection/credentials come from the `EventBusConnection` /
  `EventBusUserName` / `EventBusPassword` config keys.
- **Hangfire** (Postgres storage) runs background/scheduled jobs. `ConnectLive.Core.Api` enqueues jobs via
  `IBackgroundJobClient` and hosts the dashboard at `/workers`; `ConnectLive.Notification.Api` and
  `ConnectLive.Newsletter.Api` each run a dedicated Hangfire *server* listening to one named queue
  (`"email"`, `"newsletter"`), and job methods route to the right process via a `[Hangfire.Queue("...")]`
  attribute (see `Workers.EmailWorker`).

### Host bootstrap is duplicated per service, not shared

`ConnectLive.Core.Api` and `ConnectLive.Notification.Api` each define their own near-identical
`CreateWebHostBuilder<T>()` extension (`HostExtensions` in Core.Api, `MyCustomWebHosting` in Notification.Api)
that wires config sources (appsettings + env + command line) and Serilog→Seq logging, tagging log entries with
the assembly name of a marker type (`ClassInfo`). `ConnectLive.Newsletter.Api` has not been migrated to this
helper and still calls the plain `WebApplication.CreateBuilder` — the three hosts are not currently
consistent; when changing host bootstrap behavior, check all three.

### Known gaps — do not treat these as intended conventions to copy

- `ExceptionHandlerMiddleware` / `UseCustomException()` exist in `ConnectLive.Application` but are commented
  out in `ConnectLive.Core.Api`'s `Program.cs` — there is currently no global exception handling wired up, and
  `SendExceptionToAdmin` in the middleware is unimplemented (`throw new NotImplementedException()`).
- The `Result<T>` success/fail envelope (`ConnectLive.Portal.Shared`) is used by `QuestionController` but not
  by `UserController`, `SessionController`, or `LeaderBoardController`, which return raw `Ok(...)`.
- `SessionController`, `LeaderBoardController`, and `QuestionController` are still stubs/mocks (hardcoded
  strings or generated mock data), not wired to MediatR or the database.
