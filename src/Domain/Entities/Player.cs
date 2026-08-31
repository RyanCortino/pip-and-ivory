using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Player : BaseAuditableEntity<PlayerId>
{
    public string? DisplayName { get; set; }

    public GameStatistics BlockGameStats { get; private set; } = GameStatistics.None;

    public GameStatistics DrawGameStats { get; private set; } = GameStatistics.None;
}
