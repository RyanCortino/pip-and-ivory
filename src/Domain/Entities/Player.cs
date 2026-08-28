namespace PipAndIvory.Domain.Entities;

public readonly record struct PlayerId(Guid Value) { }

public class Player : BaseAuditableEntity<PlayerId>
{
    public string? DisplayName { get; set; }

    public int BlockGamesPlayed { get; set; }

    public int BlockGamesWon { get; set; }

    public int HighestBlockScore { get; set; }

    public int DrawGamesPlayed { get; set; }

    public int DrawGamesWon { get; set; }

    public int HighestDrawScore { get; set; }
}
