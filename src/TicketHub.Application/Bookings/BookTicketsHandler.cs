using TicketHub.Application.Abstractions;
using TicketHub.Domain.Common;
using TicketHub.Domain.Errors;
using TicketHub.Domain.ValueObjects;

namespace TicketHub.Application.Bookings;

/// <summary>Use case: a customer buys tickets for an event.</summary>
public sealed class BookTicketsHandler(IEventRepository events, TimeProvider clock)
{
    /// <summary>Validates the request, applies the domain rules, and saves the booking.</summary>
    public async Task<Result<BookingDto>> HandleAsync(BookTicketsCommand command, CancellationToken ct)
    {
        var email = EmailAddress.Create(command.CustomerEmail);
        if (email.IsFailure)
            return Result<BookingDto>.Failure(email.Error!);

        var evt = await events.GetByIdAsync(command.EventId, ct);
        if (evt is null)
            return Result<BookingDto>.Failure(DomainErrors.Event.NotFound);

        var booking = evt.Book(email.Value!, command.Quantity, clock.GetUtcNow());
        if (booking.IsFailure)
            return Result<BookingDto>.Failure(booking.Error!);

        await events.SaveBookingAsync(evt, booking.Value!, ct);
        return Result<BookingDto>.Success(BookingDto.From(booking.Value!));
    }
}
