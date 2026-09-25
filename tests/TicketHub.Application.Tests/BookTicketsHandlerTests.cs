using TicketHub.Application.Bookings;
using TicketHub.Domain.Entities;
using TicketHub.Domain.Errors;

namespace TicketHub.Application.Tests;

public sealed class BookTicketsHandlerTests
{
    private static readonly DateTimeOffset Now = new(2027, 1, 10, 12, 0, 0, TimeSpan.Zero);

    private readonly Event opera =
        Event.Restore(Guid.NewGuid(), "Opera Gala", "Grand Opera House", Now.AddMonths(3), 250, 200);

    [Fact]
    public async Task Handle_WithValidRequest_SavesAndReturnsBooking()
    {
        var repository = new InMemoryEventRepository(opera);
        var handler = new BookTicketsHandler(repository, new FixedTimeProvider(Now));

        var result = await handler.HandleAsync(new(opera.Id, "Fan@Example.com", 2), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("fan@example.com", result.Value!.CustomerEmail);
        Assert.Single(repository.SavedBookings);
        Assert.Equal(48, opera.SeatsRemaining);
    }

    [Fact]
    public async Task Handle_UnknownEvent_ReturnsNotFound()
    {
        var handler = new BookTicketsHandler(new InMemoryEventRepository(), new FixedTimeProvider(Now));

        var result = await handler.HandleAsync(new(Guid.NewGuid(), "fan@example.com", 1), CancellationToken.None);

        Assert.Equal(DomainErrors.Event.NotFound, result.Error);
    }

    [Fact]
    public async Task Handle_InvalidEmail_ReturnsErrorAndSavesNothing()
    {
        var repository = new InMemoryEventRepository(opera);
        var handler = new BookTicketsHandler(repository, new FixedTimeProvider(Now));

        var result = await handler.HandleAsync(new(opera.Id, "nope", 1), CancellationToken.None);

        Assert.Equal(DomainErrors.Email.InvalidFormat, result.Error);
        Assert.Empty(repository.SavedBookings);
    }
}
