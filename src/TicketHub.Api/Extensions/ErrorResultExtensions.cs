using Microsoft.AspNetCore.Mvc;
using TicketHub.Domain.Common;
using TicketHub.Domain.Errors;

namespace TicketHub.Api.Extensions;

/// <summary>Turns domain errors into RFC 9457 problem responses.</summary>
public static class ErrorResultExtensions
{
    /// <summary>Maps an error to a problem response with a matching HTTP status.</summary>
    public static ObjectResult ToProblem(this Error error, ControllerBase controller)
    {
        var status = StatusFor(error);
        var problem = new ProblemDetails
        {
            Status = status,
            Title = error.Message,
            Type = $"https://tickethub.example/errors/{error.Code}",
        };
        problem.Extensions["code"] = error.Code;

        return controller.StatusCode(status, problem);
    }

    private static int StatusFor(Error error) => error switch
    {
        _ when error == DomainErrors.Event.NotFound => StatusCodes.Status404NotFound,
        _ when error == DomainErrors.Event.SoldOut => StatusCodes.Status409Conflict,
        _ when error == DomainErrors.Event.NotEnoughSeats => StatusCodes.Status409Conflict,
        _ when error == DomainErrors.Event.AlreadyStarted => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status400BadRequest,
    };
}
