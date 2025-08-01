using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.Api.Infrastructure;

internal sealed class ServiceUnavailableExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<ServiceUnavailableExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ServiceTemporailyUnavailableException ex503)
        {
            return false;
        }

        logger.LogError(exception, "ServiceTemporailyUnavailableException occurred");

        httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        httpContext.Response.Headers.Append(
            "Retry-After",
            ex503.SuggestedRetry.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture));

        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status503ServiceUnavailable,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.4",
                Detail = "A transitory error has occured. " +
                        "Please try again after the number of seconds " +
                        "in the 'retry-after' header has elapsed."
            }
        };

        return await problemDetailsService.TryWriteAsync(context);
    }
}
