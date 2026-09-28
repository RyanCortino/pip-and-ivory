using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Infrastructure.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        // Key mapping (Game.Id is a GameId value-object)
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id).HasConversion(gameId => gameId.Value, value => (GameId)value);

        // Configure the GameVariant property as an owned entity
        builder.OwnsOne(v => v.GameVariant);

        //Configure the Participants collection as an owned entity
        builder.OwnsMany(
            g => g.Participants,
            participantsBuilder =>
            {
                participantsBuilder.WithOwner().HasForeignKey("GameId");

                participantsBuilder.HasKey("Id");

                participantsBuilder
                    .Property(p => p.Id)
                    .HasConversion(id => id.Value, value => (PlayerId)value);

                participantsBuilder
                    .Property<GameId>("GameId")
                    .HasConversion(gameId => gameId.Value, value => (GameId)value);
            }
        );

        builder.Navigation(g => g.Participants).AutoInclude();

        //// Configure the Rounds collection as an owned entity
        //builder.OwnsMany(
        //    g => g.Rounds,
        //    roundsBuilder =>
        //    {
        //        roundsBuilder.WithOwner().HasForeignKey("GameId");

        //        roundsBuilder
        //            .Property<GameId>("GameId")
        //            .HasConversion(gid => (Guid)gid, value => (GameId)value);

        //        roundsBuilder.HasKey("Id");

        //        roundsBuilder
        //            .Property(r => r.Id)
        //            .HasConversion(id => id.Value, value => new RoundId(value));

        //        roundsBuilder.OwnsMany(r => r.Boneyard);
        //    }
        //);
    }
}
