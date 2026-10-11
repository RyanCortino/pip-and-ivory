using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.Entities.AggregateRoots;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;
using PipAndIvory.Infrastructure.Data.ValueConverters;

namespace PipAndIvory.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Game aggregate root.
/// Establishes the complete Game aggregate hierarchy with owned entities.
/// </summary>
public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        // Primary Key
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasConversion<ReferenceTypeIdConverter<GameId>>();

        // Owned Entities
        builder.OwnsOne(v => v.GameVariant);
        builder.OwnsMany(g => g.RoundParticipants);

        // Rounds Collection
        builder.OwnsMany(g => g.Rounds, ConfigureGameRounds());

        // Navigation
        builder.Navigation(g => g.Rounds).AutoInclude();
    }

    private static Action<OwnedNavigationBuilder<Game, Round>> ConfigureGameRounds()
    {
        return roundsBuilder =>
        {
            roundsBuilder.ToTable("Rounds");
            roundsBuilder.WithOwner().HasForeignKey("GameId");

            // Primary key
            roundsBuilder.HasKey("Id");
            roundsBuilder.Property(r => r.Id).HasConversion<ReferenceTypeIdConverter<RoundId>>();

            // Foreign Key
            roundsBuilder
                .Property<GameId>("GameId")
                .HasConversion<ReferenceTypeIdConverter<GameId>>();

            // TurnOrder
            roundsBuilder.OwnsMany(r => r.TurnOrder);

            // Moves
            roundsBuilder.OwnsMany(r => r.Moves, ConfigureRoundMoves());

            // Tableau
            roundsBuilder.OwnsOne(r => r.Tableau, ConfigureRoundTableau());
        };
    }

    private static Action<OwnedNavigationBuilder<Round, Move>> ConfigureRoundMoves()
    {
        return builder =>
        {
            builder.ToTable("RoundMoves");
            builder.WithOwner().HasForeignKey("RoundId");

            // Player Id
            builder.Property(m => m.PlayerId).HasConversion<ReferenceTypeIdConverter<PlayerId>>();

            // Move Type
            builder.Property(m => m.Type).HasConversion<int>();

            // Sequence Place
            builder.Property(m => m.SequenceIndex);

            // Tile Played
            builder.OwnsOne(t => t.Tile);
            //    .Property(m => m.Tile)
            //    .HasConversion(
            //        bone => bone == null ? (int?)null : bone.Pip1,
            //        pip1 => null
            //    )
            //    .HasColumnName("Tile_Pip1");
            //movesBuilder.Property<int?>("Tile_Pip2");

            // Played At Branch
            builder.Property(m => m.BranchId);
        };
    }

    private static Action<OwnedNavigationBuilder<Round, Tableau>> ConfigureRoundTableau()
    {
        return tableauBuilder =>
        {
            tableauBuilder.ToTable("Tableaus");
            tableauBuilder.WithOwner().HasForeignKey("RoundId");

            tableauBuilder.HasKey("Id");
            tableauBuilder.Property(t => t.Id).HasConversion<ReferenceTypeIdConverter<RoundId>>();

            // Arms
            tableauBuilder.OwnsMany(t => t.Arms, ConfigureTableauArms());

            // PlayerHands
            tableauBuilder.OwnsMany(t => t.PlayerHands, ConfigureTableauPlayerHands());

            // StockPile
            tableauBuilder.OwnsMany(t => t.StockPile);
            //                    t => t.StockPile,
            //                    stockPileBuilder =>
            //                    {
            //                        stockPileBuilder.ToTable("TableauStockPile");
            //                        stockPileBuilder.WithOwner().HasForeignKey("TableauId");
            //                        stockPileBuilder.Property(b => b.Pip1);
            //                        stockPileBuilder.Property(b => b.Pip2);
            //                    }
            //                );

            // Spinner
            tableauBuilder.OwnsOne(t => t.Spinner);
            //                    .Property(t => t.Spinner)
            //                    .HasConversion(
            //                        spinner => spinner == null ? (int?)null : spinner.Pip1,
            //                        pip1 => null
            //                    )
            //                    .HasColumnName("Spinner_Pip1");
            //                tableauBuilder.Property<int?>("Spinner_Pip2");
        };
    }

    private static Action<OwnedNavigationBuilder<Tableau, Hand>> ConfigureTableauPlayerHands()
    {
        return handsBuilder =>
        {
            handsBuilder.ToTable("TableauPlayerHands");
            handsBuilder.WithOwner().HasForeignKey("TableauId");

            handsBuilder.HasKey("Id");

            handsBuilder.Property(h => h.Id).HasConversion<ReferenceTypeIdConverter<PlayerId>>();

            //handsBuilder.OwnsMany<Bone>(
            //    h => h.Bones,
            //    bonesBuilder =>
            //    {
            //        bonesBuilder.ToTable("HandBones");
            //        bonesBuilder.WithOwner().HasForeignKey("HandId");
            //        bonesBuilder.Property(b => b.Pip1);
            //        bonesBuilder.Property(b => b.Pip2);
            //    }
            //);
        };
    }

    private static Action<OwnedNavigationBuilder<Tableau, Arm>> ConfigureTableauArms()
    {
        return armsBuilder =>
        {
            armsBuilder.ToTable("TableauArms");
            armsBuilder.WithOwner().HasForeignKey("TableauId");

            armsBuilder.HasKey("Id");

            armsBuilder.Property(a => a.Id);

            armsBuilder.OwnsMany(a => a.Branches, ConfigureArmBranches());
        };
    }

    private static Action<OwnedNavigationBuilder<Arm, Branch>> ConfigureArmBranches()
    {
        return branchesBuilder =>
        {
            branchesBuilder.ToTable("ArmBranches");
            branchesBuilder.WithOwner().HasForeignKey("ArmId");

            branchesBuilder.HasKey("Id");

            branchesBuilder.Property(b => b.Id);

            branchesBuilder.Property(b => b.OpenEnd);
        };
    }
}
