using Application.Abstractions.Messaging;
using Application.Animals.Create;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Domain.Users;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Web.Api.Contracts.Animals.CreateDog.v1;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

using CreateDogRequestV1 = Web.Api.Contracts.Animals.CreateDog.v1.CreateDogRequest;
using CreateDogRequestV2 = Web.Api.Contracts.Animals.CreateDog.v2.CreateDogRequest;

namespace Web.Api.Endpoints.Animals;

internal sealed class CreateDog : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        ApiVersionSet versionSet = app.NewApiVersionSet()
            .HasDeprecatedApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        app.MapPost("v1/dog", async (
            CreateDogRequestV1 request,
            ICommandHandler<CreateDogCommand, Guid> handler,
            CancellationToken cancellationToken) =>

            (await handler.Handle(ToCommand(request), cancellationToken))
                .Match(Results.Ok, CustomResults.Problem))
            .WithTags(Tags.Animals)
            .RequireAuthorization()
            .WithApiVersionSet(versionSet)
            .WithOpenApi();

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/include-metadata?view=aspnetcore-9.0&tabs=minimal-apis#include-openapi-metadata-for-endpoints
        app.MapPost("v2/dog", async (
            CreateDogRequestV2 request,
            ICommandHandler<CreateDogCommand, Guid> handler,
            CancellationToken cancellationToken) =>

            (await handler.Handle(ToCommand(request), cancellationToken))
                .Match(
                    id => Results.Created($"animal/dog/{id}", id),
                    //id => Results.Created($"animal/dog/{id}", null),
                    //dog => Results.Created($"animal/dog/{dog.Id}", ToResponse(dog)),
                    CustomResults.Problem))
            .RequireAuthorization()
            .WithApiVersionSet(versionSet)
            //.WithGroupName("v2") Puts into documents, no group name means in all docs
            .WithTags(Tags.Animals)
            .WithName("CreateDog") // seems to do nothing
            //.WithSummary("Creates a new dog.") // This obscures url in OpenAPI based docs.
            .WithDescription("Creates a dog and returns the created resource with a location header.")
            .Accepts<CreateDogRequestV1>("application/json")
            .Produces<Guid>(201)
            .Produces<ValidationProblemDetails>(400)
            .Produces(401)
            .Produces<ProblemDetails>(503);
    }

    private static CreateDogCommand ToCommand(CreateDogRequestV1 request) => new(request.Name, request.HairColour1, request.HairColour2, request.HairPattern);
    private static CreateDogCommand ToCommand(CreateDogRequestV2 request) => new(request.Name, request.HairColour1, request.HairColour2, request.HairPattern);
    //private static CreateDogResponse ToResponse(Domain.Animals.Dog dog) => new(dog.Id, true, dog.Name, dog.MakeSound(), dog.HairColourDescription());
}



