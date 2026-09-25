namespace TicketHub.Application.Bookings;

/// <summary>A request to buy tickets for an event.</summary>
/// <param name="EventId">The event to book.</param>
/// <param name="CustomerEmail">The buyer's email address.</param>
/// <param name="Quantity">Number of tickets, from 1 to 10.</param>
public sealed record BookTicketsCommand(Guid EventId, string CustomerEmail, int Quantity);
