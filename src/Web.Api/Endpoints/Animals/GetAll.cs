using Application.Abstractions.Messaging;
using Application.Animals.GetAll;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Animals;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("animals", async (
            IQueryHandler<GetAnimalsQuery, List<AnimalResponse>> handler,
            CancellationToken cancellationToken) =>

            (await handler.Handle(new(), cancellationToken))
                .Match(Results.Ok, CustomResults.Problem)
        )
        .WithTags(Tags.Animals)
        .RequireAuthorization();
    }
}
