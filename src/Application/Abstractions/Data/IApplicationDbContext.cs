using Domain.Animals;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Cat> Cats { get; }
    DbSet<Dog> Dogs { get; }
    DbSet<Bird> Birds { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <remarks>
/// Relies in ArchitectureTests to make sure queries cant use IApplicationDbContext.
/// DBSet properties are useful for creating hydrated entities, Database for SQL queries.
/// </remarks>
public interface IReadOnlyApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Cat> Cats { get; }
    DbSet<Dog> Dogs { get; }
    DbSet<Bird> Birds { get; }

    DatabaseFacade Database { get; }
}

