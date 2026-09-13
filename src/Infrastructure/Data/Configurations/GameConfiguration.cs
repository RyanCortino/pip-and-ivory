using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Infrastructure.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        // Key mapping (Game.Id is a GameId value-object)
        builder.HasKey(g => g.Id);

        builder
            .Property(g => g.Id)
            .HasConversion(gameId => gameId.Value, value => new GameId(value));

        // Configure the GameVariant property as an owned entity
        builder.OwnsOne(v => v.GameVariant);
    }
}
