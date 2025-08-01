using System.Reflection;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Web.Api.Documentation;
using Web.Api.Infrastructure;

namespace Web.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        return services;
    }

    internal static IServiceCollection AddOpenApiWithOptions(this IServiceCollection services)
    {
        // Revisit in.NET 10 with improvements to OpenAPI
        services.AddOpenApi("v1", o => {
            //o.ShouldInclude = (description) => description.GroupName == null || description.GroupName == DocumentName;
            o.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            o.AddOperationTransformer<ExampleOperationFilter>();
            o.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                //operation.Description... check description, and add appropriate responses
                operation.Responses.Add("500", new OpenApiResponse { Description = "Internal server error" });
                return Task.CompletedTask;
            });
        });
        services.AddOpenApi("v2");
        services.AddOpenApi("internal", o =>
        {
            o.ShouldInclude = (description) =>
            {
                // Include only as long as no tag contains "Animal"
                ITagsMetadata? tagsAttribute = description.ActionDescriptor.EndpointMetadata
                    .OfType<ITagsMetadata>()
                    .FirstOrDefault();

                if (tagsAttribute is null)
                {
                    return true;
                }

                return !tagsAttribute.Tags.Any(tag => tag.Contains("Animal", StringComparison.OrdinalIgnoreCase));
            };
        });

        return services;
    }

    internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            o.AddSecurityRequirement(securityRequirement);

            string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Web.Api.contracts.xml"));
        });

        return services;
    }

    internal static IServiceCollection AddExceptionsAndProblemDetails(this IServiceCollection services)
    {
        services.AddExceptionHandler<NotImplementedExceptionHandler>();
        services.AddExceptionHandler<ServiceUnavailableExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                HttpRequest request = context.HttpContext.Request;
                Microsoft.AspNetCore.Mvc.ProblemDetails pd = context.ProblemDetails!;

                pd.Instance ??= request.GetDisplayUrl();

                ILogger logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("ProblemDetails");

                using (logger.BeginScope(new Dictionary<string, object?>
                {
                    ["ProblemDetailsStatusCode"] = pd.Status,
                    ["ProblemDetailsType"] = pd.Type,
                    ["ProblemDetailsTitle"] = pd.Title,
                    ["ProblemDetailsDetail"] = pd.Detail,
                    ["ProblemDetailsInstance"] = pd.Instance,
                    ["RequestMethod"] = request.Method,
                    ["RequestPath"] = request.Path,
                    ["RequestQueryString"] = request.QueryString.ToString()
                }))
                {
                    logger.LogWarning("ProblemDetails generated for request: {Request}", request.Path);
                }
            };
        });

        return services;
    }
}
