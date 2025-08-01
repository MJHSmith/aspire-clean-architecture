using Application.Abstractions.Messaging;
using Application.Animals.Create;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Animals;

internal sealed class CreateBird : IEndpoint
{
    public readonly record struct Request(string Name, uint WingspanInCentimters)
    {
        public CreateBirdCommand ToCommand() => new(Name, WingspanInCentimters);
    };

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("bird", async (
            Request request,
            ICommandHandler<CreateBirdCommand, Guid> handler,
            CancellationToken cancellationToken) =>

            (await handler.Handle(request.ToCommand(), cancellationToken))
                .Match(Results.Ok, CustomResults.Problem)
        )
        .WithTags(Tags.Animals)
        .RequireAuthorization();
    }
}
