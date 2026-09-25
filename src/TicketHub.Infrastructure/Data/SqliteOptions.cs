namespace TicketHub.Infrastructure.Data;

/// <summary>Where the SQLite database lives.</summary>
/// <param name="DatabasePath">Absolute path to the <c>.db</c> file.</param>
public sealed record SqliteOptions(string DatabasePath)
{
    /// <summary>The connection string for <see cref="DatabasePath"/>.</summary>
    public string ConnectionString => $"Data Source={DatabasePath};Foreign Keys=True";
}
