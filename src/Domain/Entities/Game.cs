using System.Runtime.CompilerServices;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Game : BaseAuditableEntity<GameId>
{
    /// <summary>
    /// The selected game variant/mode for this game instance.
    /// Defaults to <see cref="GameVariant.Block"/>.
    /// </summary>
    public GameVariant GameVariant { get; set; } = GameVariant.Block;

    public IList<Participant> Participants { get; private set; } = new List<Participant>();

    public static Game Start(GameVariant gameVariant, IList<PlayerId> playerIds)
    {
        var newGameId = GameId.New;

        var participants = playerIds
            .Select(pid => new Participant { Id = pid, GameId = newGameId })
            .ToList();

        var game = new Game
        {
            Id = newGameId,
            GameVariant = gameVariant,
            Participants = participants,
        };

        return game;
    }
}

public class Participant : BaseEntity<PlayerId>
{
    public required GameId GameId { get; set; }

    public int CurrentScore { get; set; }

    public bool IsWinner { get; set; }

    public Game Game { get; set; } = null!;
}

public class Round : BaseEntity<RoundId>;
