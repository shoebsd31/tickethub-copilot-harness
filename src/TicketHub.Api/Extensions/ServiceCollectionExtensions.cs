using TicketHub.Application.Abstractions;
using TicketHub.Application.Bookings;
using TicketHub.Application.Events;
using TicketHub.Infrastructure.Data;
using TicketHub.Infrastructure.Repositories;

namespace TicketHub.Api.Extensions;

/// <summary>Composition root: wires every layer into the DI container.</summary>
public static class ServiceCollectionExtensions
{
    /// <summary>Registers TicketHub's application and infrastructure services.</summary>
    public static IServiceCollection AddTicketHub(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var configuredPath = configuration["Database:Path"] ?? "../../data/tickets.db";
        var databasePath = Path.GetFullPath(Path.Combine(environment.ContentRootPath, configuredPath));

        services.AddSingleton(new SqliteOptions(databasePath));
        services.AddSingleton<SqliteConnectionFactory>();
        services.AddSingleton<DatabaseInitializer>();
        services.AddScoped<IEventRepository, SqliteEventRepository>();

        services.AddSingleton(TimeProvider.System);
        services.AddScoped<EventQueries>();
        services.AddScoped<BookTicketsHandler>();

        return services;
    }
}
