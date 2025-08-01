using SharedKernel;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Testing;

internal sealed class BadRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("badrequest",  () =>
        {
            return CustomResults.Problem(Result.Failure(new Error("Test.Validation","Please check your input and try again.", ErrorType.Validation)));
        })
        .WithTags("Testing");
    }
}
