using TicketHub.Domain.ValueObjects;

namespace TicketHub.Domain.Entities;

/// <summary>A customer's purchase of one or more tickets for a single event.</summary>
public sealed class Booking
{
    private Booking(
        Guid id, Guid eventId, EmailAddress customer, int quantity, BookingStatus status, DateTimeOffset bookedAt)
    {
        Id = id;
        EventId = eventId;
        Customer = customer;
        Quantity = quantity;
        Status = status;
        BookedAt = bookedAt;
    }

    /// <summary>Unique booking id.</summary>
    public Guid Id { get; }

    /// <summary>The event the tickets are for.</summary>
    public Guid EventId { get; }

    /// <summary>The customer who made the booking.</summary>
    public EmailAddress Customer { get; }

    /// <summary>Number of tickets in the booking.</summary>
    public int Quantity { get; }

    /// <summary>Current lifecycle state.</summary>
    public BookingStatus Status { get; }

    /// <summary>When the booking was made.</summary>
    public DateTimeOffset BookedAt { get; }

    /// <summary>Creates a new confirmed booking. Only <see cref="Event"/> may call this.</summary>
    internal static Booking Create(Guid eventId, EmailAddress customer, int quantity, DateTimeOffset now) =>
        new(Guid.NewGuid(), eventId, customer, quantity, BookingStatus.Confirmed, now);

    /// <summary>Rebuilds a booking from storage.</summary>
    public static Booking Restore(
        Guid id, Guid eventId, EmailAddress customer, int quantity, BookingStatus status, DateTimeOffset bookedAt) =>
        new(id, eventId, customer, quantity, status, bookedAt);
}
