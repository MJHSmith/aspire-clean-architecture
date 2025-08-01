namespace Web.Api.Endpoints.Testing;

internal sealed class ServerError : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("servererror", () =>
        {
            throw new Exception("an unexpected unhandled exception.");
        })
        .WithTags("Testing");
    }
}
