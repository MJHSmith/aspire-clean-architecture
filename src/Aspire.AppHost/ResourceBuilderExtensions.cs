using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Aspire.AppHost;
internal static class ResourceBuilderExtensions
{
    internal static IResourceBuilder<T> WithSwaggerUI<T>(this IResourceBuilder<T> builder)
        where T : IResourceWithEndpoints
    {
        return builder.WithOpenApiDocs(
            name: "swagger-ui-docs",
            displayName: "Swagger API Documentation",
            openApiUiPath: "swagger");
    }

    internal static IResourceBuilder<T> WithScalar<T>(this IResourceBuilder<T> builder)
    where T : IResourceWithEndpoints
    {
        return builder.WithOpenApiDocs(
            name: "scalar-docs",
            displayName: "Scalar API Documentation",
            openApiUiPath: "scalar/v1");
    }

    internal static IResourceBuilder<T> WithReDoc<T>(this IResourceBuilder<T> builder)
    where T : IResourceWithEndpoints
    {
        return builder.WithOpenApiDocs(
            name: "redoc-docs",
            displayName: "ReDoc API Documentation",
            openApiUiPath: "api-docs");
    }

    private static IResourceBuilder<T> WithOpenApiDocs<T>(
            this IResourceBuilder<T> builder,
            string name,
            string displayName,
            string openApiUiPath)
        where T : IResourceWithEndpoints
    {
        return builder.WithCommand(
            name,
            displayName,
            executeCommand:  _ =>
            {
                try
                {
                    EndpointReference endpoint = builder.GetEndpoint("https");
                    string url = $"{endpoint.Url}/{openApiUiPath}";
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    return Task.FromResult(new ExecuteCommandResult { Success = true });
                }
                catch (Exception e)
                {
                    return Task.FromResult(new ExecuteCommandResult { Success = false, ErrorMessage = e.ToString() });
                }
            },
            new CommandOptions()
            {
                UpdateState = context => context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy ? ResourceCommandState.Enabled : ResourceCommandState.Disabled,
                IconName = "Document",
                IconVariant = IconVariant.Filled
            }
        );
    }
}
