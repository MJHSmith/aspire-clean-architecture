using Application.Abstractions.Messaging;
using Application.Animals.Create;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Animals;

internal sealed class CreateCat : IEndpoint
{
    public readonly record struct Request(string Name, string HairColourDescription)
    {
        public CreateCatCommand ToCommand() => new(Name, HairColourDescription);
    };

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("cat", async (
            Request request,
            ICommandHandler<CreateCatCommand, Guid> handler,
            CancellationToken cancellationToken) =>

            (await handler.Handle(request.ToCommand(), cancellationToken))
                .Match(Results.Ok, CustomResults.Problem)
        )
        .WithTags(Tags.Animals)
        .RequireAuthorization();
    }
}
