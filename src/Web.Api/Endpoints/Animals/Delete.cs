using Application.Abstractions.Messaging;
using Application.Animals.Delete;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Animals;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("animal/{type}/{id:guid}", async (
            string type,
            Guid id,
            ICommandHandler<DeleteAnimalCommand> handler,
            CancellationToken cancellationToken) =>

            (await handler.Handle(new(type, id), cancellationToken))
                .Match(Results.NoContent, CustomResults.Problem)
        )
        .WithTags(Tags.Animals)
        .RequireAuthorization();
    }
}
