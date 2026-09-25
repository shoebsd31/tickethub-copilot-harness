using Microsoft.Data.Sqlite;

namespace TicketHub.Infrastructure.Data;

/// <summary>Opens connections to the TicketHub database.</summary>
public sealed class SqliteConnectionFactory(SqliteOptions options)
{
    /// <summary>Opens a new connection. The caller owns and disposes it.</summary>
    public async Task<SqliteConnection> OpenAsync(CancellationToken ct)
    {
        var connection = new SqliteConnection(options.ConnectionString);
        await connection.OpenAsync(ct);
        return connection;
    }
}
