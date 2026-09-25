using Microsoft.AspNetCore.Mvc;
using TicketHub.Api.Extensions;
using TicketHub.Application.Bookings;
using TicketHub.Application.Events;

namespace TicketHub.Api.Controllers;

/// <summary>Browse events and buy tickets.</summary>
[ApiController]
[Route("api/events")]
public sealed class EventsController(EventQueries queries, BookTicketsHandler bookTickets) : ControllerBase
{
    /// <summary>Lists every event, soonest first.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<EventDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(CancellationToken ct) =>
        Ok(await queries.ListAsync(ct));

    /// <summary>Gets one event.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<EventDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(Guid id, CancellationToken ct)
    {
        var result = await queries.GetAsync(id, ct);
        return result.Match<IActionResult>(Ok, error => error.ToProblem(this));
    }

    /// <summary>Buys tickets for an event.</summary>
    [HttpPost("{id:guid}/bookings")]
    [ProducesResponseType<BookingDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> BookAsync(Guid id, BookTicketsRequest request, CancellationToken ct)
    {
        var command = new BookTicketsCommand(id, request.CustomerEmail, request.Quantity);
        var result = await bookTickets.HandleAsync(command, ct);

        return result.Match<IActionResult>(
            booking => Created($"/api/events/{id}/bookings/{booking.Id}", booking),
            error => error.ToProblem(this));
    }
}

/// <summary>Body of a booking request.</summary>
/// <param name="CustomerEmail">The buyer's email address.</param>
/// <param name="Quantity">Number of tickets, from 1 to 10.</param>
public sealed record BookTicketsRequest(string CustomerEmail, int Quantity);
