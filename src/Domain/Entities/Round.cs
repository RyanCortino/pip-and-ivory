using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Round : BaseEntity<RoundId>
{
    public static Round Create(IEnumerable<Bone> initialBoneyard)
    {
        var round = new Round { Id = RoundId.New, Stock = [.. initialBoneyard] };

        return round;
    }

    public IList<Hand> PlayerHands { get; private set; } = [];

    public IList<PlayerId> TurnOrder { get; private set; } = [];

    public IList<Move> Moves { get; private set; } = [];

    public IList<Bone> Stock { get; private set; } = [];

    public bool IsComplete { get; private set; }

    public bool IsBlocked => _consecutivePasses >= TurnOrder.Count;

    private readonly LineOfPlay _lineOfPlay = new();

    private int _turnIndex;

    private int _consecutivePasses;

    private PlayerId? _lastPlayerToDraw;

    private int NextSequence => Moves.Count + 1;

    public PlayerId CurrentTurn => TurnOrder[_turnIndex];

    public Score CurrentScore =>
        new(PlayerHands.ToDictionary(h => h.Id, h => h.Bones.Sum(b => b.Weight)));

    public IReadOnlyList<OpenEnd> GetOpenEnds => _lineOfPlay.OpenEnds;

    public void ShuffleBoneyard()
    {
        Stock = [.. Stock.OrderBy(_ => Guid.NewGuid())];
    }

    public void StartRound(IList<Participant> participants)
    {
        DealPlayerHands(participants);

        // Determine the turn order based on the highest weight first bone in each player's hand
        List<PlayerId> turnOrder =
        [
            .. PlayerHands.OrderByDescending(h => h.Bones.First().Weight).Select(h => h.Id),
        ];
        TurnOrder = turnOrder;
    }

    public RoundResult Complete(RoundEndReason roundEndReason)
    {
        IsComplete = true;

        var result = new RoundResult(roundEndReason, CurrentScore);

        return result;
    }

    public void DealPlayerHands(IList<Participant> participants)
    {
        var handSize = participants.Count <= 3 ? 7 : 5;

        var hands = new List<Hand>();

        // Assuming you have access to the game entity and its participants
        foreach (var participant in participants)
        {
            List<Bone> bones = [.. Stock.Take(handSize)];

            hands.Add(new Hand { Id = participant.Id, Bones = bones });

            if (Stock is List<Bone> boneyard)
                boneyard.RemoveRange(0, bones.Count);
        }

        if (PlayerHands is List<Hand> playerHands)
            playerHands.AddRange(hands);
    }

    public void Pass(GameId gameId, PlayerId playerId)
    {
        EnsureTurn(playerId);

        RecordMove(
            gameId,
            playerId,
            Move.Create(playerId, MoveType.Pass, null, null, NextSequence)
        );

        _consecutivePasses++;

        AdvanceTurnIndex();
    }

    public void DrawFromStock(GameId gameId, PlayerId playerId)
    {
        EnsureTurn(playerId);

        if (CanDraw(playerId) is not true)
        {
            Pass(gameId, playerId);

            return;
        }

        _lastPlayerToDraw = playerId;

        var drawnBone = Stock.First();

        Stock.RemoveAt(0);

        var hand = PlayerHands.First(h => h.Id == playerId);

        hand.Bones.Add(drawnBone);

        RecordMove(
            gameId,
            playerId,
            Move.Create(playerId, MoveType.Draw, null, null, NextSequence)
        );
    }

    public void PlayTile(GameId gameId, PlayerId playerId, Bone bone, BranchId branchId)
    {
        EnsureTurn(playerId);

        var hand = PlayerHands.First(h => h.Id == playerId);

        if (!hand.Bones.Contains(bone))
            throw new InvalidOperationException(
                "Player does not have the specified bone in their hand."
            );

        _lineOfPlay.Place(bone, branchId);

        hand.Bones.Remove(bone);

        RecordMove(
            gameId,
            playerId,
            Move.Create(playerId, MoveType.PlayMatch, bone, branchId, NextSequence)
        );

        _consecutivePasses = 0;

        AdvanceTurnIndex();
    }

    private void AdvanceTurnIndex()
    {
        _turnIndex = (_turnIndex + 1) % TurnOrder.Count;
    }

    private void EnsureTurn(PlayerId player)
    {
        if (player != CurrentTurn)
        {
            throw new InvalidOperationException($"It's not {player}'s turn.");
        }
    }

    private void RecordMove(GameId game, PlayerId player, Move move)
    {
        Moves.Add(move);

        AddDomainEvent(new MoveMadeDomainEvent(game, player, move));
    }

    private bool CanDraw(PlayerId playerId) => Stock.Count > 0 && !AlreadyDrewThisTurn(playerId);

    private bool AlreadyDrewThisTurn(PlayerId playerId) =>
        _lastPlayerToDraw is not null && playerId == _lastPlayerToDraw;
}
