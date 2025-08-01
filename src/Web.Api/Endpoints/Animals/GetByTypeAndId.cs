using System.Text.Json.Serialization;
using Application.Abstractions.Messaging;
using Application.Animals.Get;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Animals;

internal sealed class GetByTypeAndId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("animal/{type}/{id:guid}", async (
            string type,
            Guid id,
            IQueryHandler<GetAnimalQuery, AnimalResponse> handler,
            CancellationToken cancellationToken) =>

            (await handler.Handle(new GetAnimalQuery(type, id), cancellationToken))
                .Match(Results.Ok, CustomResults.Problem)
        )
        .WithTags(Tags.Animals)
        .RequireAuthorization();
    }
}

