using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;

namespace Web.Api.Middleware;

/// <summary>
/// Middleware that enriches log entries with request-specific context, such as correlation IDs and user identifiers.
/// </summary>
/// <remarks>This middleware extracts a correlation ID from the incoming HTTP request headers or generates one if
/// none is provided. It also attempts to retrieve the user ID from the authenticated user's claims. These values are
/// added to the logging scope, allowing them to be included in log entries for the duration of the request. The
/// correlation ID is also used to track requests across distributed systems.</remarks>
/// <param name="next"></param>
/// <param name="logger"></param>
public class RequestContextLoggingMiddleware(RequestDelegate next, ILogger<RequestContextLoggingMiddleware> logger)
{
    private const string CorrelationIdHeaderName = "CorrelationId";

    /// <summary>
    /// Processes the HTTP request by adding contextual information, such as correlation ID and user ID, to the logging
    /// scope.
    /// </summary>
    /// <remarks>This middleware adds a correlation ID and, if available, a user ID to the logging scope for
    /// the duration of the request. The correlation ID is always included, while the user ID is added only if it can be
    /// determined from the request context.</remarks>
    /// <param name="context">The <see cref="HttpContext"/> representing the current HTTP request.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation of processing the HTTP request.</returns>
    public Task Invoke(HttpContext context)
    {
        var data = new Dictionary<string, object>
        {
            ["CorrelationId"] = GetCorrelationId(context)
        };

        string? userId = GetUserId(context);
        if (userId is not null)
        {
            Activity.Current?.SetTag("user.id", userId);

            data["UserId"] = userId;
        }

        using (logger.BeginScope(data))
        {
            return next.Invoke(context);
        }
    }

    private static string? GetUserId(HttpContext context)
    {
        return context.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(
            CorrelationIdHeaderName,
            out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}
