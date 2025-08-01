using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Aspire.Microsoft.EntityFrameworkCore.SqlServer;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Database;
using Infrastructure.DomainEvents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        InfrastructureOptions options)
    {
        if (options.JwtOptions is null)
        {
            throw new ArgumentException("JwtOptions of InfrastructureOptions cannot be null.", nameof(options));
        }

        return AddInfrastructure(services,
                o =>
                {
                    o.Issuer = options.JwtOptions.Issuer;
                    o.Audience = options.JwtOptions.Audience;
                    o.Secret = options.JwtOptions.Secret;
                    o.ExpirationInMinutes = options.JwtOptions.ExpirationInMinutes;
                }, options.ConnectionString);
    }


    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Action<JwtOptions> configureOptions,
        string? databaseConnectionString)
    {
        if (string.IsNullOrEmpty(databaseConnectionString)){
            throw new ArgumentException("Database connection string must be provided.", nameof(databaseConnectionString));
        }

        services
            .AddOptionsWithValidateOnStart<JwtOptions>() // Adds to DI
            .Configure(configureOptions) // sets values to those provided
            .ValidateDataAnnotations()
            .Validate(config =>
            {
                if (config.Audience == "Bob")
                {
                    return false;
                }

                return true;
            }, "Audience must not be bob."); // Validate full options provided

        ServiceProvider sp = services.BuildServiceProvider();

        return services
            .AddServices()
            .AddDatabase(databaseConnectionString)
            .AddHealthChecks(databaseConnectionString)
            .AddAuthenticationInternal(sp.GetService<IOptions<JwtOptions>>()!)
            .AddAuthorizationInternal();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .UseSnakeCaseNamingConvention());        

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IReadOnlyApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    private static IServiceCollection AddHealthChecks(
        this IServiceCollection services, string connectionString)
    {
        services
            .AddHealthChecks()
            .AddSqlServer(connectionString);

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IOptions<JwtOptions> jwtOptions)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret!)),
                    ValidIssuer = jwtOptions.Value.Issuer,
                    ValidAudience = jwtOptions.Value.Audience,
                    ClockSkew = TimeSpan.Zero
                };

            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddScoped<PermissionProvider>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
