using TicketHub.Domain.Entities;
using TicketHub.Domain.Errors;
using static TicketHub.Domain.Tests.EventFactory;

namespace TicketHub.Domain.Tests;

public sealed class EventTests
{
    [Fact]
    public void Create_WithValidDetails_StartsWithNoSeatsSold()
    {
        var result = Event.Create("Jazz Night", "Blue Note Hall", Now.AddMonths(1), capacity: 120);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value!.SeatsSold);
        Assert.Equal(120, result.Value.SeatsRemaining);
    }

    [Theory]
    [InlineData("", 100)]
    [InlineData("Jazz Night", 0)]
    public void Create_WithInvalidDetails_Fails(string name, int capacity)
    {
        var result = Event.Create(name, "Blue Note Hall", Now.AddMonths(1), capacity);

        Assert.Equal(DomainErrors.Event.InvalidDetails, result.Error);
    }

    [Fact]
    public void Book_WithSeatsAvailable_ReducesRemainingSeats()
    {
        var evt = WithSeats(capacity: 100, seatsSold: 10);

        var result = evt.Book(Customer, quantity: 4, Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(4, result.Value!.Quantity);
        Assert.Equal(86, evt.SeatsRemaining);
    }

    [Fact]
    public void Book_WhenEventIsFull_ReturnsSoldOut()
    {
        var evt = WithSeats(capacity: 100, seatsSold: 100);

        var result = evt.Book(Customer, quantity: 1, Now);

        Assert.Equal(DomainErrors.Event.SoldOut, result.Error);
        Assert.Equal(100, evt.SeatsSold);
    }

    [Fact]
    public void Book_WhenQuantityExceedsRemainingSeats_ReturnsNotEnoughSeats()
    {
        var evt = WithSeats(capacity: 100, seatsSold: 98);

        var result = evt.Book(Customer, quantity: 4, Now);

        Assert.Equal(DomainErrors.Event.NotEnoughSeats, result.Error);
        Assert.Equal(98, evt.SeatsSold);
    }

    [Fact]
    public void Book_TakingExactlyTheLastSeats_Succeeds()
    {
        var evt = WithSeats(capacity: 100, seatsSold: 96);

        var result = evt.Book(Customer, quantity: 4, Now);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, evt.SeatsRemaining);
    }

    [Fact]
    public void Book_AfterEventStarted_ReturnsAlreadyStarted()
    {
        var evt = WithSeats(capacity: 100, seatsSold: 0);

        var result = evt.Book(Customer, quantity: 1, evt.StartsAt.AddMinutes(1));

        Assert.Equal(DomainErrors.Event.AlreadyStarted, result.Error);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Book_WithQuantityOutsideLimits_ReturnsInvalidQuantity(int quantity)
    {
        var evt = WithSeats(capacity: 100, seatsSold: 0);

        var result = evt.Book(Customer, quantity, Now);

        Assert.Equal(DomainErrors.Booking.InvalidQuantity, result.Error);
    }
}
