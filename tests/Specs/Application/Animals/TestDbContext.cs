using Application.Abstractions.Data;
using Domain.Animals;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Specs.Application.Animals;

public class TestDbContext : DbContext, IReadOnlyApplicationDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    public DbSet<Cat> Cats { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Dog> Dogs { get; set; }
    public DbSet<Bird> Birds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public static TestDbContext Create()
    {
        DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        var context = new TestDbContext(options);
        return context;
    }
}
