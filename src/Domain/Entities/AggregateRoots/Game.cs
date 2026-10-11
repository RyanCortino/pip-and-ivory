using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities.AggregateRoots;

public class Game : BaseAuditableEntity<GameId>
{
    public static Game Create(GameVariant gameVariant, IList<PlayerId> playerIds)
    {
        var game = new Game
        {
            Id = GameId.New,
            GameVariant = gameVariant,
            GameStatus = GameStatus.InProgress,
        };

        foreach (var playerId in playerIds)
        {
            game.RoundParticipants.Add(RoundParticipant.From(playerId));
        }

        game.AddDomainEvent(
            new GameStartedDomainEvent(game.Id, [.. game.RoundParticipants.Select(p => p.PlayerId)])
        );

        return game;
    }

    public GameStatus GameStatus { get; set; }

    public GameVariant GameVariant { get; set; } = GameVariant.Block;

    public IList<RoundParticipant> RoundParticipants { get; private set; } = [];

    public IList<Round> Rounds { get; private set; } = [];

    public Round CurrentRound => Rounds.Count > 0 ? Rounds[^1] : null!;

    public void StartNewRound()
    {
        // Create a new round
        var round = Round.Create(Bone.StandardDoubleSixSet);

        round.StartRound(RoundParticipants);

        // Raise a domain event to indicate that the round has started
        round.AddDomainEvent(new RoundStartedDomainEvent(Id, round.Id, round.CurrentTurn));

        Rounds.Add(round);
    }

    public void Play(PlayerId playerId, Bone bone)
    {
        EnsureInProgress();

        CurrentRound.PlayTile(Id, playerId, bone);
    }

    public void Play(PlayerId playerId, Bone bone, Arm arm, Branch branch)
    {
        EnsureInProgress();

        CurrentRound.PlayTile(Id, playerId, bone, arm, branch);

        // Check if the player has emptied their hand
        if (CurrentRound.Tableau.PlayerHands.First(h => h.Id == playerId).Bones.Count == 0)
        {
            CompleteRound(RoundEndReason.Domino);
        }
    }

    public void Pass(PlayerId playerId)
    {
        EnsureInProgress();

        CurrentRound.Pass(Id, playerId);

        // Check if the round is blocked after the player passes
        if (CurrentRound.IsBlocked)
        {
            CompleteRound(RoundEndReason.Blocked);
        }
    }

    public void DrawFromStock(PlayerId playerId)
    {
        EnsureInProgress();

        CurrentRound.Draw(Id, playerId);
    }

    public void CompleteRound(RoundEndReason roundEndReason)
    {
        EnsureInProgress();

        var result = CurrentRound.Complete(roundEndReason);

        AddDomainEvent(new RoundEndedDomainEvent(Id, result));
    }

    private void EnsureInProgress()
    {
        if (GameStatus != GameStatus.InProgress)
            throw new InvalidOperationException(
                "The game is not in progress and cannot be modified."
            );
    }
}
