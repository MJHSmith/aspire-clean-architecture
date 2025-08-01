namespace Web.Api.Endpoints.Testing;

internal sealed class NotImplemented : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("notimplemented", () =>
        {
            throw new NotImplementedException("This endpoint is not implemented yet.");
        })
        .WithTags("Testing");
    }
}
