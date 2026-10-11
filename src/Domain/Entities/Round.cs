using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Round : BaseEntity<RoundId>
{
    public static Round Create(IEnumerable<Bone> initialBoneyard)
    {
        var round = new Round { Id = RoundId.New };

        round.Tableau = Tableau.From(initialBoneyard, round.Id);

        return round;
    }

    public IReadOnlyList<PlayerId> TurnOrder { get; private set; } = [];

    public IReadOnlyList<Move> Moves => _moves.AsReadOnly();
    private readonly List<Move> _moves = [];

    public Tableau Tableau { get; private set; } = null!;

    public bool IsComplete { get; private set; }

    public bool IsBlocked => _consecutivePasses >= TurnOrder.Count;

    private int _turnIndex;

    private int _consecutivePasses;

    private PlayerId? _lastPlayerToDraw;

    public PlayerId CurrentTurn => TurnOrder[_turnIndex];

    public Score CurrentScore =>
        new(Tableau!.PlayerHands.ToDictionary(h => h.Id, h => h.Bones.Sum(b => b.Weight)));

    public void StartRound(IList<RoundParticipant> participants)
    {
        DealPlayerHands(participants);

        // Determine the turn order based on the highest weight first bone in each player's hand
        List<PlayerId> turnOrder =
        [
            .. Tableau.PlayerHands.OrderByDescending(h => h.Bones.First().Weight).Select(h => h.Id),
        ];

        TurnOrder = turnOrder;
    }

    public RoundResult Complete(RoundEndReason roundEndReason)
    {
        IsComplete = true;

        var result = new RoundResult(roundEndReason, CurrentScore);

        return result;
    }

    public void DealPlayerHands(IList<RoundParticipant> participants)
    {
        var handSize = participants.Count <= 3 ? 7 : 5;

        var hands = new List<Hand>();

        // Assuming you have access to the game entity and its participants
        foreach (var participant in participants)
        {
            List<Bone> bones = [.. Tableau.StockPile.Take(handSize)];

            hands.Add(Hand.From(participant.PlayerId, bones));

            if (Tableau.StockPile is List<Bone> boneyard)
                boneyard.RemoveRange(0, bones.Count);
        }

        if (Tableau.PlayerHands is List<Hand> playerHands)
            playerHands.AddRange(hands);
    }

    public void Draw(GameId gameId, PlayerId playerId)
    {
        EnsureTurn(playerId);

        if (CanDraw(playerId))
        {
            _lastPlayerToDraw = playerId;

            var playersHand = Tableau.PlayerHands.First(h => h.Id == playerId);

            var drawnTile = Tableau.DrawTo(playersHand);

            RecordMove(gameId, playerId, Move.Draw(playerId, drawnTile, Moves.Count + 1));
        }

        AdvanceTurnIndex();
    }

    public void Pass(GameId gameId, PlayerId playerId)
    {
        EnsureTurn(playerId);

        RecordMove(gameId, playerId, Move.Pass(playerId, Moves.Count + 1));

        AdvanceTurnIndex(passedThisTurn: true);
    }

    // SetSpinner
    public void PlayTile(GameId gameId, PlayerId playerId, Bone bone)
    {
        EnsureTurn(playerId);

        var hand = Tableau.PlayerHands.First(h => h.Id == playerId);

        Tableau.SetSpinner(bone);

        hand.Release(bone);

        RecordMove(gameId, playerId, Move.SetSpinner(playerId, bone, Moves.Count + 1));

        AdvanceTurnIndex();
    }

    // SetMatch, SetHeavy
    public void PlayTile(GameId gameId, PlayerId playerId, Bone bone, Arm arm, Branch branch)
    {
        EnsureTurn(playerId);

        var hand = Tableau.PlayerHands.First(h => h.Id == playerId);

        Tableau.SetMatch(bone, arm, branch);

        hand.Release(bone);

        RecordMove(gameId, playerId, Move.PlayMatch(playerId, bone, branch, Moves.Count + 1));

        AdvanceTurnIndex();
    }

    private void AdvanceTurnIndex(bool passedThisTurn = false)
    {
        _consecutivePasses = !passedThisTurn ? 0 : _consecutivePasses++;

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
        _moves.Add(move);

        AddDomainEvent(new MoveMadeDomainEvent(game, player, move));
    }

    private bool CanDraw(PlayerId playerId) =>
        Tableau.StockPile.Count > 0 && !AlreadyDrewThisTurn(playerId);

    private bool AlreadyDrewThisTurn(PlayerId playerId) =>
        _lastPlayerToDraw is not null && playerId == _lastPlayerToDraw;
}
