using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Infrastructure;

internal sealed class NotImplementedExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<NotImplementedExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotImplementedException)
        {
            return false;
        }

        logger.LogError(exception, "Unhandled NotImplementedException occurred");

        httpContext.Response.StatusCode = StatusCodes.Status501NotImplemented;
        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status501NotImplemented,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2",
                Detail = "This function has not yet been implemented. Please contact the administrator."
            }
        };

        return await problemDetailsService.TryWriteAsync(context);
    }
}
