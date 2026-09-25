using TicketHub.Domain.Common;

namespace TicketHub.Domain.Errors;

/// <summary>Catalog of every business error the domain can produce.</summary>
public static class DomainErrors
{
    /// <summary>Errors about events and their seating.</summary>
    public static class Event
    {
        /// <summary>No event exists with the requested id.</summary>
        public static readonly Error NotFound =
            new("Event.NotFound", "The requested event was not found.");

        /// <summary>The event has a missing name or a non-positive capacity.</summary>
        public static readonly Error InvalidDetails =
            new("Event.InvalidDetails", "An event needs a name and a capacity of at least one seat.");

        /// <summary>Every seat has been sold.</summary>
        public static readonly Error SoldOut =
            new("Event.SoldOut", "This event is sold out.");

        /// <summary>Some seats remain, but fewer than the customer asked for.</summary>
        public static readonly Error NotEnoughSeats =
            new("Event.NotEnoughSeats", "Not enough seats remain for this booking.");

        /// <summary>The event has already started, so tickets can no longer be sold.</summary>
        public static readonly Error AlreadyStarted =
            new("Event.AlreadyStarted", "Tickets can't be booked after the event has started.");
    }

    /// <summary>Errors about individual bookings.</summary>
    public static class Booking
    {
        /// <summary>The ticket quantity is outside the allowed range.</summary>
        public static readonly Error InvalidQuantity =
            new("Booking.InvalidQuantity", "A booking must contain between 1 and 10 tickets.");
    }

    /// <summary>Errors about customer email addresses.</summary>
    public static class Email
    {
        /// <summary>The email address is empty or malformed.</summary>
        public static readonly Error InvalidFormat =
            new("Email.InvalidFormat", "Please provide a valid email address.");
    }
}
