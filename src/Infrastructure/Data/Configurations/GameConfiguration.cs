using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;
using PipAndIvory.Infrastructure.Data.Configurations.ValueConverters;

namespace PipAndIvory.Infrastructure.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        // Key mapping (Game.Id is a GameId value-object)
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id).HasConversion<ReferenceTypeIdConverter<GameId>>();

        // Configure the GameVariant property as an owned entity
        builder.OwnsOne(v => v.GameVariant);

        //Configure the Participants collection as an owned entity
        builder.OwnsMany(
            g => g.Participants,
            participantsBuilder =>
            {
                participantsBuilder.ToTable("Participants");

                participantsBuilder.WithOwner().HasForeignKey("GameId");

                participantsBuilder.HasKey("Id");

                participantsBuilder
                    .Property(p => p.Id)
                    .HasConversion<ReferenceTypeIdConverter<PlayerId>>();

                participantsBuilder
                    .Property<GameId>("GameId")
                    .HasConversion<ReferenceTypeIdConverter<GameId>>();
            }
        );

        // Configure the Rounds collection as an owned entity
        builder.OwnsMany(
            g => g.Rounds,
            roundsBuilder =>
            {
                roundsBuilder.ToTable("Rounds");

                roundsBuilder.WithOwner().HasForeignKey("GameId");

                roundsBuilder.HasKey("Id");

                roundsBuilder
                    .Property(r => r.Id)
                    .HasConversion<ReferenceTypeIdConverter<RoundId>>();

                roundsBuilder
                    .Property<GameId>("GameId")
                    .HasConversion<ReferenceTypeIdConverter<GameId>>();

                roundsBuilder.OwnsMany<Hand>(
                    r => r.PlayerHands,
                    handsBuilder =>
                    {
                        handsBuilder.ToTable("Hands");

                        handsBuilder.WithOwner().HasForeignKey("RoundId");

                        handsBuilder.HasKey("Id");

                        handsBuilder
                            .Property(h => h.Id)
                            .HasConversion<ReferenceTypeIdConverter<PlayerId>>();

                        handsBuilder
                            .Property<RoundId>("RoundId")
                            .HasConversion<ReferenceTypeIdConverter<RoundId>>();

                        handsBuilder.OwnsMany<Bone>(h => h.Bones);
                    }
                );

                roundsBuilder.OwnsMany<PlayerId>(r => r.TurnOrder);

                roundsBuilder.OwnsMany<Bone>(r => r.Boneyard);

                roundsBuilder.OwnsMany<Move>(r => r.Moves);

                roundsBuilder.Navigation(r => r.PlayerHands).AutoInclude();
            }
        );

        builder.Navigation(g => g.Participants).AutoInclude();
        builder.Navigation(g => g.Rounds).AutoInclude();
    }
}
