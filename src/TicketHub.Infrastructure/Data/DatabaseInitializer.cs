using System.Reflection;

namespace TicketHub.Infrastructure.Data;

/// <summary>Creates or resets the database from the embedded <c>data/seed.sql</c>.</summary>
public sealed class DatabaseInitializer(SqliteConnectionFactory connections, SqliteOptions options)
{
    /// <summary>Seeds the database only if it has no tables yet.</summary>
    public async Task EnsureCreatedAsync(CancellationToken ct)
    {
        await using var connection = await connections.OpenAsync(ct);
        await using var check = connection.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'events'";
        var exists = Convert.ToInt64(await check.ExecuteScalarAsync(ct)) > 0;

        if (!exists)
            await ResetAsync(ct);
    }

    /// <summary>Drops everything and reloads the demo data.</summary>
    public async Task ResetAsync(CancellationToken ct)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(options.DatabasePath)!);

        await using var connection = await connections.OpenAsync(ct);
        await using var command = connection.CreateCommand();
        command.CommandText = await ReadSeedScriptAsync(ct);
        await command.ExecuteNonQueryAsync(ct);
    }

    private static async Task<string> ReadSeedScriptAsync(CancellationToken ct)
    {
        var assembly = Assembly.GetExecutingAssembly();
        await using var stream = assembly.GetManifestResourceStream("TicketHub.seed.sql")
            ?? throw new InvalidOperationException("Embedded seed.sql is missing from the build.");
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync(ct);
    }
}
