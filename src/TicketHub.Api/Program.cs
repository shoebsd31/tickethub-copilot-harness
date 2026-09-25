using TicketHub.Api.Extensions;
using TicketHub.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTicketHub(builder.Configuration, builder.Environment);
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

var app = builder.Build();

var database = app.Services.GetRequiredService<DatabaseInitializer>();

// `dotnet run -- --reset-db` restores the demo data and exits.
if (args.Contains("--reset-db"))
{
    await database.ResetAsync(CancellationToken.None);
    app.Logger.LogInformation("Demo database reset from data/seed.sql");
    return;
}

await database.EnsureCreatedAsync(CancellationToken.None);

app.UseExceptionHandler();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

/// <summary>Entry point; public so integration tests can host the app.</summary>
public partial class Program;
