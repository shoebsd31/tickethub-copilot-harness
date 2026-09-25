using TicketHub.Application.Abstractions;
using TicketHub.Domain.Common;
using TicketHub.Domain.Errors;

namespace TicketHub.Application.Events;

/// <summary>Read-only use cases for browsing events.</summary>
public sealed class EventQueries(IEventRepository events)
{
    /// <summary>Lists every event, soonest first.</summary>
    public async Task<IReadOnlyList<EventDto>> ListAsync(CancellationToken ct)
    {
        var all = await events.ListAsync(ct);
        return all.Select(EventDto.From).ToList();
    }

    /// <summary>Gets one event by id.</summary>
    public async Task<Result<EventDto>> GetAsync(Guid id, CancellationToken ct)
    {
        var evt = await events.GetByIdAsync(id, ct);
        return evt is null
            ? Result<EventDto>.Failure(DomainErrors.Event.NotFound)
            : Result<EventDto>.Success(EventDto.From(evt));
    }
}
