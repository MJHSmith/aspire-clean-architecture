using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Web.Api.Extensions;

/// <summary>
/// Provides extension methods for applying database migrations.
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Applies any pending migrations for the context to the database.
    /// </summary>
    /// <param name="app">The application builder.</param>
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
