namespace TicketHub.Domain.Entities;

/// <summary>Lifecycle state of a booking.</summary>
public enum BookingStatus
{
    /// <summary>The booking is paid and its seats are held.</summary>
    Confirmed,
}
