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

    public void Play(PlayerId playerId, Bone bone, BranchId branchId)
    {
        EnsureInProgress();

        CurrentRound.PlayTile(Id, playerId, bone, branchId);

        // Check if the player has emptied their hand
        if (CurrentRound.PlayerHands.First(h => h.Id == playerId).Bones.Count == 0)
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

        if (GameVariant != GameVariant.Draw)
            throw new InvalidOperationException(
                "Only games with the 'Draw' variant can draw from the stock."
            );

        CurrentRound.DrawFromStock(Id, playerId);
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
