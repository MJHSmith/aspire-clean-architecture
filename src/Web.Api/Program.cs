using System.Reflection;
using System.Security.Claims;
using System.Text;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Web.Api;
using Web.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(new InfrastructureOptions( // Ideally configuration info should ony be read in applicaiton root
        builder.Configuration.GetSection("Jwt").Get<JwtOptions>(),
        builder.Configuration.GetConnectionString("database"))); // Note: "datebase" is the name of the resores in Aspire.AppHost

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.MapDefaultEndpoints();
app.MapEndpoints();

if (app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("UnitTesting"))
{
    app.UseSwaggerWithUi();
    app.UseOpenApiWithScalaraAndReDoc();
    app.ApplyMigrations();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCors();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestContextLogging(); // Add after auth, so that we have userId in claims then able to log it

//app.MapControllers(); // Only needed for WebAPI and MVC controllers

await app.RunAsync();

/// <summary>
/// Note: Required for functional and integration tests to work.
/// </summary>
public partial class Program { }
