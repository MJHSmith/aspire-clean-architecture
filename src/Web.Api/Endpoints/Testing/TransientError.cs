using SharedKernel;

namespace Web.Api.Endpoints.Testing;

internal sealed class TransientError : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("transienterror", () =>
        {
            throw new ServiceTemporailyUnavailableException(TimeSpan.FromMinutes(1), "We are having trouble with our 3rd party provider. Please try again later.");
        })
        .WithTags("Testing");
    }
}
