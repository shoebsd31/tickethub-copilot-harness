using TicketHub.Domain.Entities;

namespace TicketHub.Application.Abstractions;

/// <summary>Persistence for events and the bookings made against them.</summary>
public interface IEventRepository
{
    /// <summary>Lists every event, soonest first.</summary>
    Task<IReadOnlyList<Event>> ListAsync(CancellationToken ct);

    /// <summary>Finds one event, or returns null when it doesn't exist.</summary>
    Task<Event?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>Saves a new booking and the event's updated seat count in one transaction.</summary>
    Task SaveBookingAsync(Event evt, Booking booking, CancellationToken ct);
}
