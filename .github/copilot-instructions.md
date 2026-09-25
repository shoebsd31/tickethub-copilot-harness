# TicketHub: instructions for AI coding agents

TicketHub is a .NET 10 Web API for selling event tickets. It uses a layered, domain-driven design.

## Architecture

- `src/TicketHub.Domain`: entities, value objects, and business rules. **No package dependencies.**
- `src/TicketHub.Application`: use cases (queries and handlers). Depends only on Domain.
- `src/TicketHub.Infrastructure`: SQLite persistence via `Microsoft.Data.Sqlite`.
- `src/TicketHub.Api`: thin controllers. The composition root is `Extensions/ServiceCollectionExtensions.cs`.
- `data/seed.sql`: the schema and demo data. `data/tickets.db` is the local database.

## Commands

- Build: `dotnet build`
- Test: `dotnet test`
- Run the API: `dotnet run --project src/TicketHub.Api` (listens on http://localhost:5080)

## Rules

1. **Bug fixes are test-first.** Write a failing test that reproduces the bug, run `dotnet test`, and show it
   failing *before* changing production code. Then fix the code and run the tests again.
2. **Never finish with a red suite.** A task isn't done until `dotnet test` passes with no failures or skips.
3. **Business rules live in the Domain** and report failures by returning `Result<T>` with an error from
   `DomainErrors`. Never throw exceptions for business rule violations.
4. Keep every `.cs` file under 150 lines. Add XML doc comments to public members.
5. Test names follow `Method_Scenario_ExpectedOutcome`.
6. Do not edit `data/tickets.db` or `data/seed.sql` unless explicitly asked.
7. Keep changes focused. Don't rename or reformat code unrelated to the task.
