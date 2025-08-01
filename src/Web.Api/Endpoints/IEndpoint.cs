namespace Web.Api.Endpoints;

/// <summary>
/// Defines a contract for mapping an endpoint to an <see cref="IEndpointRouteBuilder"/>.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="app">The endpoint route builder to map to.</param>
    void MapEndpoint(IEndpointRouteBuilder app);
}
