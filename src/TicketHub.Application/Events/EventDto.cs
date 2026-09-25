using TicketHub.Domain.Entities;

namespace TicketHub.Application.Events;

/// <summary>An event as returned to API clients.</summary>
public sealed record EventDto(
    Guid Id,
    string Name,
    string Venue,
    DateTimeOffset StartsAt,
    int Capacity,
    int SeatsSold,
    int SeatsRemaining)
{
    /// <summary>Maps a domain event to its DTO.</summary>
    public static EventDto From(Event evt) =>
        new(evt.Id, evt.Name, evt.Venue, evt.StartsAt, evt.Capacity, evt.SeatsSold, evt.SeatsRemaining);
}
