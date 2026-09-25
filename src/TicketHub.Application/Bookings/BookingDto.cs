using TicketHub.Domain.Entities;

namespace TicketHub.Application.Bookings;

/// <summary>A booking as returned to API clients.</summary>
public sealed record BookingDto(
    Guid Id,
    Guid EventId,
    string CustomerEmail,
    int Quantity,
    string Status,
    DateTimeOffset BookedAt)
{
    /// <summary>Maps a domain booking to its DTO.</summary>
    public static BookingDto From(Booking booking) =>
        new(booking.Id, booking.EventId, booking.Customer.Value, booking.Quantity,
            booking.Status.ToString(), booking.BookedAt);
}
