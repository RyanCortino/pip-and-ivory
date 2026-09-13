using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Infrastructure.Data.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participants");

        // Key mapping (Participant.Id is a PlayerId value-object)
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(id => id.Value, value => new PlayerId(value));

        // Foreign key back to Game: use a shadow FK column storing GameId Guid
        builder
            .Property<GameId>("GameId")
            .HasConversion(gid => gid.Value, value => new GameId(value));

        //builder
        //    .HasOne<Game>()
        //    .WithMany(g => g.Participants)
        //    .HasForeignKey("GameId")
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
