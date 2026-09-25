---
description: Add a read-only API endpoint to TicketHub, following the team's layering and test rules
agent: agent
---

Add this endpoint to TicketHub: ${input:endpoint:e.g. GET /api/events/{id}/availability}

Follow these steps in order:

1. Put any new business logic in `src/TicketHub.Domain` and return `Result<T>` for failures.
2. Add a query method and DTO in `src/TicketHub.Application`, next to the existing ones.
3. Write unit tests for the new logic first, and run `dotnet test` to see them fail.
4. Add the controller action in `src/TicketHub.Api/Controllers`, mapping errors with `ToProblem`.
5. Run `dotnet test` until everything passes.
6. Finish with a short summary: the files you changed and an example request.
