using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web.Api.Endpoints;

namespace Web.Api.Extensions;

/// <summary>
/// Provides extension methods for registering and mapping API endpoints.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Registers all non-abstract, non-interface types implementing <see cref="IEndpoint"/> from the specified assembly as transient services.
    /// </summary>
    /// <param name="services">The service collection to add endpoints to.</param>
    /// <param name="assembly">The assembly to scan for endpoint implementations.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    /// Maps all registered <see cref="IEndpoint"/> instances to the application's endpoint route builder.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <param name="routeGroupBuilder">An optional route group builder to use for mapping endpoints.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    /// <summary>
    /// Adds a permission requirement to the specified route handler.
    /// </summary>
    /// <param name="app">The route handler builder.</param>
    /// <param name="permission">The required permission.</param>
    /// <returns>The <see cref="RouteHandlerBuilder"/> with the permission requirement applied.</returns>
    public static RouteHandlerBuilder HasPermission(this RouteHandlerBuilder app, string permission)
    {
        return app.RequireAuthorization(permission);
    }
}
