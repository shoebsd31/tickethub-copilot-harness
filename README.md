# TicketHub

A small .NET 10 Web API for selling event tickets.

## Requirements

- .NET 10 SDK (`dotnet --version` should print 10.x)
- VS Code with the C# Dev Kit and GitHub Copilot

## Getting started

```bash
dotnet test                              # run the test suite
dotnet run --project src/TicketHub.Api   # start the API on http://localhost:5080
```

Open `src/TicketHub.Api/TicketHub.Api.http` to send example requests.

## Endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| GET | `/api/events` | List all events |
| GET | `/api/events/{id}` | Get one event |
| POST | `/api/events/{id}/bookings` | Buy tickets (`customerEmail`, `quantity` 1–10) |
| GET | `/health` | Health check |

Errors are returned as RFC 9457 problem details with a stable `code`, e.g. `Event.SoldOut`.

## Data

The API uses SQLite at `data/tickets.db`, created from `data/seed.sql`.
To restore the demo data: `dotnet run --project src/TicketHub.Api -- --reset-db`.
