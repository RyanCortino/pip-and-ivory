using System.Runtime.CompilerServices;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Game : BaseAuditableEntity<GameId>
{
    public static Game Create(GameVariant gameVariant, IList<PlayerId> playerIds)
    {
        var newGameId = GameId.New;

        var participants = playerIds.Select(pid => new Participant { Id = pid }).ToList();

        var game = new Game
        {
            Id = newGameId,
            GameVariant = gameVariant,
            GameStatus = GameStatus.InProgress,
        };

        // Add participants to the game
        foreach (var playerId in playerIds)
        {
            var participant = new Participant { Id = playerId };

            game.Participants.Add(participant);
        }

        // Raise the domain event for game start
        game.AddDomainEvent(new GameStartedDomainEvent(game));

        return game;
    }

    public GameStatus GameStatus { get; set; }

    public GameVariant? GameVariant { get; set; }

    /// <summary>
    /// The list of players in this game instance.
    /// </summary>
    public IList<Participant> Participants { get; private set; } = new List<Participant>();
}


//    /// <summary>
//    /// The list of rounds played in this game instance.
//    /// </summary>
//    public IList<Round> Rounds { get; private set; } = new List<Round>();

//    //public Round CurrentRound => Rounds.Count > 0 ? Rounds[^1] : null!;
//}
