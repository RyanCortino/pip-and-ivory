using System.Runtime.CompilerServices;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Round : BaseEntity<RoundId>
{
    public static Round Create(IEnumerable<Bone> initialBoneyard)
    {
        var round = new Round { Id = RoundId.New, Boneyard = [.. initialBoneyard] };

        return round;
    }

    public IList<Hand> PlayerHands { get; private set; } = [];

    public IList<PlayerId> TurnOrder { get; private set; } = [];

    public IList<Move> Moves { get; private set; } = [];

    public IList<Bone> Boneyard { get; private set; } = [];

    //public LineOfPlay LineOfPlay { get; set; } = new LineOfPlay();

    private int _turnIndex;

    public PlayerId CurrentTurn => TurnOrder[_turnIndex];

    public void EndTurn()
    {
        _turnIndex = _turnIndex++;
    }

    public void ShuffleBoneyard()
    {
        Boneyard = [.. Boneyard.OrderBy(_ => Guid.NewGuid())];
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

    public void DealPlayerHands(IList<Participant> participants)
    {
        var handSize = participants.Count <= 3 ? 7 : 5;

        var hands = new List<Hand>();

        // Assuming you have access to the game entity and its participants
        foreach (var participant in participants)
        {
            List<Bone> bones = [.. Boneyard.Take(handSize)];

            hands.Add(new Hand { Id = participant.Id, Bones = bones });

            if (Boneyard is List<Bone> boneyard)
                boneyard.RemoveRange(0, bones.Count);
        }

        if (PlayerHands is List<Hand> playerHands)
            playerHands.AddRange(hands);
    }
}
