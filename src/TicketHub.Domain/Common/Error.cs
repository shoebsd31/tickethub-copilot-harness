namespace TicketHub.Domain.Common;

/// <summary>A business rule violation, identified by a stable, machine-readable code.</summary>
/// <param name="Code">Stable code in the form <c>Area.Reason</c>, e.g. <c>Event.SoldOut</c>.</param>
/// <param name="Message">Human-readable explanation, safe to show to API clients.</param>
public sealed record Error(string Code, string Message);
