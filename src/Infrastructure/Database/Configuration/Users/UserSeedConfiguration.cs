using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Animals;
using Domain.Users;
using Domain.ValueObjects;
using Infrastructure.Authentication;
using Infrastructure.Database.Configuration.Animals;
using Infrastructure.Database.Configuration.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Users;

internal sealed class UserSeedData : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User()
            {
                Id = Guid.Parse("8000ba35-6cf7-4aef-90f7-097f8f6a3b13"),
                FirstName = Name.Create("API").Value,
                LastName = Name.Create("User").Value,
                Email = "a@b.com", 
                PasswordHash = "BE18F3BE7CD3799DF493F45B9118F3EE4A5BF512A96DC326907257D238B44628-29841CFA820416A7FEFB8CB02C56301B" // 5Babylon!
            });
    }
}
