using Scalar.AspNetCore;

namespace Web.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring Swagger, OpenAPI, Scalar, and ReDoc in a <see cref="WebApplication"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Enables Swagger and Swagger UI middleware for the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    /// <summary>
    /// Enables OpenAPI, Scalar API Reference, and ReDoc middleware for the specified <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder UseOpenApiWithScalaraAndReDoc(this WebApplication app)
    {
        app.MapOpenApi();
        app.UseReDoc(options =>
        {
            options.DocumentTitle = "API Documentation (ReDoc)";
            options.SpecUrl = "/openapi/v1.json";
        });
        app.MapScalarApiReference();
        return app;
    }
}
