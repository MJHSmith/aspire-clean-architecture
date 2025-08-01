using Web.Api.Middleware;

namespace Web.Api.Extensions;

/// <summary>
/// Provides extension methods for registering middleware components in the application's request pipeline.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds the <see cref="RequestContextLoggingMiddleware"/> to the application's request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }
}
