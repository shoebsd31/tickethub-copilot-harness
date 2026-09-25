using TicketHub.Domain.Common;
using TicketHub.Domain.Errors;
using TicketHub.Domain.ValueObjects;

namespace TicketHub.Domain.Entities;

/// <summary>A ticketed event with a fixed number of seats. Owns the rules for selling them.</summary>
public sealed class Event
{
    /// <summary>The largest number of tickets a single booking may contain.</summary>
    public const int MaxTicketsPerBooking = 10;

    private Event(Guid id, string name, string venue, DateTimeOffset startsAt, int capacity, int seatsSold)
    {
        Id = id;
        Name = name;
        Venue = venue;
        StartsAt = startsAt;
        Capacity = capacity;
        SeatsSold = seatsSold;
    }

    /// <summary>Unique event id.</summary>
    public Guid Id { get; }

    /// <summary>Display name, e.g. "Jazz Night".</summary>
    public string Name { get; }

    /// <summary>Where the event takes place.</summary>
    public string Venue { get; }

    /// <summary>When the event starts. No tickets are sold after this moment.</summary>
    public DateTimeOffset StartsAt { get; }

    /// <summary>Total number of seats.</summary>
    public int Capacity { get; }

    /// <summary>Number of seats already sold.</summary>
    public int SeatsSold { get; private set; }

    /// <summary>Seats still available for sale.</summary>
    public int SeatsRemaining => Capacity - SeatsSold;

    /// <summary>Creates a new event with no seats sold.</summary>
    public static Result<Event> Create(string name, string venue, DateTimeOffset startsAt, int capacity) =>
        string.IsNullOrWhiteSpace(name) || capacity < 1
            ? Result<Event>.Failure(DomainErrors.Event.InvalidDetails)
            : Result<Event>.Success(new Event(Guid.NewGuid(), name.Trim(), venue.Trim(), startsAt, capacity, 0));

    /// <summary>Rebuilds an event from storage.</summary>
    public static Event Restore(
        Guid id, string name, string venue, DateTimeOffset startsAt, int capacity, int seatsSold) =>
        new(id, name, venue, startsAt, capacity, seatsSold);

    /// <summary>Sells tickets to a customer and returns the resulting booking.</summary>
    /// <param name="customer">Who is buying.</param>
    /// <param name="quantity">How many tickets, from 1 to <see cref="MaxTicketsPerBooking"/>.</param>
    /// <param name="now">The current time, used to stop sales once the event starts.</param>
    public Result<Booking> Book(EmailAddress customer, int quantity, DateTimeOffset now)
    {
        if (quantity is < 1 or > MaxTicketsPerBooking)
            return Result<Booking>.Failure(DomainErrors.Booking.InvalidQuantity);

        if (now >= StartsAt)
            return Result<Booking>.Failure(DomainErrors.Event.AlreadyStarted);

        if (SeatsSold >= Capacity)
            return Result<Booking>.Failure(DomainErrors.Event.SoldOut);

        SeatsSold += quantity;
        return Result<Booking>.Success(Booking.Create(Id, customer, quantity, now));
    }
}
