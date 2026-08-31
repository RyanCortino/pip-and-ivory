using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Infrastructure.Data.ValueConverters;

namespace PipAndIvory.Infrastructure.Data.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion<PlayerIdConverter>();

        builder.Property(t => t.DisplayName).HasMaxLength(70);

        builder.OwnsOne(
            p => p.BlockGameStats,
            gs =>
            {
                gs.Property(g => g.Played).HasColumnName("BlockGamesPlayed");
                gs.Property(g => g.Won).HasColumnName("BlockGamesWon");
                gs.Property(g => g.HighestScore).HasColumnName("BlockGamesHighestScore");
            }
        );

        builder.OwnsOne(
            p => p.DrawGameStats,
            gs =>
            {
                gs.Property(g => g.Played).HasColumnName("DrawGamesPlayed");
                gs.Property(g => g.Won).HasColumnName("DrawGamesWon");
                gs.Property(g => g.HighestScore).HasColumnName("DrawGamesHighestScore");
            }
        );
    }
}
