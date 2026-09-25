using TicketHub.Application.Abstractions;
using TicketHub.Domain.Entities;

namespace TicketHub.Application.Tests;

/// <summary>In-memory repository that records what was saved.</summary>
internal sealed class InMemoryEventRepository(params Event[] seed) : IEventRepository
{
    private readonly Dictionary<Guid, Event> events = seed.ToDictionary(e => e.Id);

    public List<Booking> SavedBookings { get; } = [];

    public Task<IReadOnlyList<Event>> ListAsync(CancellationToken ct) =>
        Task.FromResult<IReadOnlyList<Event>>(events.Values.OrderBy(e => e.StartsAt).ToList());

    public Task<Event?> GetByIdAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(events.GetValueOrDefault(id));

    public Task SaveBookingAsync(Event evt, Booking booking, CancellationToken ct)
    {
        events[evt.Id] = evt;
        SavedBookings.Add(booking);
        return Task.CompletedTask;
    }
}

/// <summary>A clock frozen at a fixed moment.</summary>
internal sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => now;
}
