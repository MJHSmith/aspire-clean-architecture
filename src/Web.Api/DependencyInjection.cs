using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.Options;
using Web.Api.Documentation;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api;

/// <summary>
/// Provides extension methods for registering presentation layer services and middleware.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds presentation layer services and middleware to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("x-api-version"),
                new QueryStringApiVersionReader("x-api-version")
            );
        });

        services.AddOpenApiWithOptions();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGenWithAuth();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (Environment.GetEnvironmentVariable("FRONTEND_ORIGIN") is not null)
                {
                    policy.WithOrigins(Environment.GetEnvironmentVariable("FRONTEND_ORIGIN")!);
                }
                else
                {
                    policy.AllowAnyOrigin();
                }

                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        //services.AddControllers(); // For WebAPI or MVC controllers

        services.AddExceptionsAndProblemDetails();

        return services;
    }
}
