using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public readonly record struct GameId(Guid Value) { }

public class Game : BaseAuditableEntity<GameId>
{
    /// <summary>
    /// The selected game variant/mode for this game instance.
    /// Defaults to <see cref="GameVariant.Block"/>.
    /// </summary>
    public GameVariant Gamemode { get; set; } = GameVariant.Block;

    public int ScoreToWin { get; private set; }

    public GameStatus Status { get; private set; } = GameStatus.InProgress;

    public IList<Participant> Participants { get; private set; } = [];

    public IList<Round> Rounds { get; private set; } = [];

    public Round CurrentRound => Rounds[^1];
}

public class Participant : BaseEntity<PlayerId>
{
    public int CurrentScore { get; private set; }

    public bool IsWinner { get; private set; }
}

public class Round : BaseEntity<RoundId>;
