namespace PipAndIvory.Domain.Entities;

public readonly record struct GameId(Guid Value) { }

/// <summary>
/// Represents a game session including the selected game variant, the target score required to win,
/// and progress tracking such as rounds played. Inherits auditing fields from <see cref="BaseAuditableEntity"/>.
/// </summary>
public class Game : BaseAuditableEntity<GameId>
{
    /// <summary>
    /// The score required for a player to win the game.
    /// Expect a non-negative integer; a value of 0 may indicate an unset or alternative win condition.
    /// </summary>
    public int ScoreToWin { get; set; }

    /// <summary>
    /// The number of rounds that have been played in this game instance.
    /// Starts at 0 and should be incremented as rounds complete.
    /// </summary>
    public int RoundsPlayed { get; set; }

    /// <summary>
    /// The selected game variant/mode for this game instance.
    /// Defaults to <see cref="GameVariant.Block"/>.
    /// </summary>
    public GameVariant Gamemode { get; set; } = GameVariant.Block;

    public IList<PlayerId> Players { get; private set; } = new List<PlayerId>();
}
