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

        foreach (var playerId in playerIds)
        {
            var participant = new Participant { Id = playerId };

            game.Participants.Add(participant);
        }

        game.AddDomainEvent(
            new GameStartedDomainEvent(game.Id, [.. game.Participants.Select(g => g.Id)])
        );

        return game;
    }

    public GameStatus GameStatus { get; set; }

    public GameVariant GameVariant { get; set; } = GameVariant.Block;

    public IList<Participant> Participants { get; private set; } = [];

    public IList<Round> Rounds { get; private set; } = [];

    public Round CurrentRound => Rounds.Count > 0 ? Rounds[^1] : null!;

    public void StartNewRound()
    {
        // Create a new round
        var round = Round.Create(Bone.StandardDoubleSixSet);

        round.ShuffleBoneyard();

        round.StartRound(Participants);

        // Raise a domain event to indicate that the round has started
        round.AddDomainEvent(new RoundStartedDomainEvent(Id, round.Id, round.CurrentTurn));

        Rounds.Add(round);
    }
}
