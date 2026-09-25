using System.Globalization;
using Microsoft.Data.Sqlite;
using TicketHub.Application.Abstractions;
using TicketHub.Domain.Entities;
using TicketHub.Infrastructure.Data;

namespace TicketHub.Infrastructure.Repositories;

/// <summary>SQLite implementation of <see cref="IEventRepository"/>.</summary>
public sealed class SqliteEventRepository(SqliteConnectionFactory connections) : IEventRepository
{
    private const string SelectEvents =
        "SELECT id, name, venue, starts_at, capacity, seats_sold FROM events";

    /// <inheritdoc />
    public async Task<IReadOnlyList<Event>> ListAsync(CancellationToken ct)
    {
        await using var connection = await connections.OpenAsync(ct);
        await using var command = connection.CreateCommand();
        command.CommandText = $"{SelectEvents} ORDER BY starts_at";

        var results = new List<Event>();
        await using var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
            results.Add(ReadEvent(reader));

        return results;
    }

    /// <inheritdoc />
    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        await using var connection = await connections.OpenAsync(ct);
        await using var command = connection.CreateCommand();
        command.CommandText = $"{SelectEvents} WHERE id = $id";
        command.Parameters.AddWithValue("$id", id.ToString());

        await using var reader = await command.ExecuteReaderAsync(ct);
        return await reader.ReadAsync(ct) ? ReadEvent(reader) : null;
    }

    /// <inheritdoc />
    public async Task SaveBookingAsync(Event evt, Booking booking, CancellationToken ct)
    {
        await using var connection = await connections.OpenAsync(ct);
        await using var transaction = (SqliteTransaction)await connection.BeginTransactionAsync(ct);

        await using (var update = connection.CreateCommand())
        {
            update.Transaction = transaction;
            update.CommandText = "UPDATE events SET seats_sold = $sold WHERE id = $id";
            update.Parameters.AddWithValue("$sold", evt.SeatsSold);
            update.Parameters.AddWithValue("$id", evt.Id.ToString());
            await update.ExecuteNonQueryAsync(ct);
        }

        await using (var insert = connection.CreateCommand())
        {
            insert.Transaction = transaction;
            insert.CommandText = """
                INSERT INTO bookings (id, event_id, customer_email, quantity, status, booked_at)
                VALUES ($id, $eventId, $email, $quantity, $status, $bookedAt)
                """;
            insert.Parameters.AddWithValue("$id", booking.Id.ToString());
            insert.Parameters.AddWithValue("$eventId", booking.EventId.ToString());
            insert.Parameters.AddWithValue("$email", booking.Customer.Value);
            insert.Parameters.AddWithValue("$quantity", booking.Quantity);
            insert.Parameters.AddWithValue("$status", booking.Status.ToString());
            insert.Parameters.AddWithValue("$bookedAt", booking.BookedAt.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'+00:00'", CultureInfo.InvariantCulture));
            await insert.ExecuteNonQueryAsync(ct);
        }

        await transaction.CommitAsync(ct);
    }

    private static Event ReadEvent(SqliteDataReader reader) =>
        Event.Restore(
            id: Guid.Parse(reader.GetString(0)),
            name: reader.GetString(1),
            venue: reader.GetString(2),
            startsAt: DateTimeOffset.Parse(reader.GetString(3), CultureInfo.InvariantCulture),
            capacity: reader.GetInt32(4),
            seatsSold: reader.GetInt32(5));
}
