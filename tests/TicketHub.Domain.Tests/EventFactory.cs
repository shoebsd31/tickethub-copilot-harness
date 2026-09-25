using TicketHub.Domain.Entities;
using TicketHub.Domain.ValueObjects;

namespace TicketHub.Domain.Tests;

/// <summary>Builds events and customers for tests.</summary>
internal static class EventFactory
{
    public static readonly DateTimeOffset Now = new(2027, 1, 10, 12, 0, 0, TimeSpan.Zero);

    public static readonly EmailAddress Customer = EmailAddress.Create("fan@example.com").Value!;

    /// <summary>An event next month with the given capacity and seats already sold.</summary>
    public static Event WithSeats(int capacity, int seatsSold) =>
        Event.Restore(Guid.NewGuid(), "Jazz Night", "Blue Note Hall", Now.AddMonths(1), capacity, seatsSold);
}
