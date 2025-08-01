using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Api.Documentation;

public class ExampleOperationFilter : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        // TODO: Get all endpoints, find what they accept => T.
        // Search for all Example<T> - for each add an example to the operation 
        if (operation.OperationId == "CreateDog")
        {
            operation
                .RequestBody.Content["application/json"]
                .Examples.Add(
                    "Example",
                    new OpenApiExample()
                    {
                        Description = "Test",
                        Value = OpenApiAnyFactory.CreateFromJson(
@"{
  ""name"": ""Buddy"",
  ""hairColour1"": ""Brown"",
  ""hairColour2"": ""Black"",
  ""hairPattern"": ""Patches""
}")

                    }
                );
        }

        return Task.CompletedTask;
    }
}
