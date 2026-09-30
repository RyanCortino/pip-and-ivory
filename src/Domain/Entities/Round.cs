using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Round : BaseEntity<RoundId>
{
    public IList<Hand> PlayerHands { get; private set; } = [];

    public IList<PlayerId> TurnOrder { get; private set; } = [];

    public IList<Bone> Boneyard { get; set; } = [];

    public IList<Move> Moves { get; private set; } = new List<Move>();

    //public LineOfPlay LineOfPlay { get; set; } = new LineOfPlay();

    private int _turnIndex;

    public PlayerId CurrentTurn => TurnOrder[_turnIndex];

    public void EndTurn()
    {
        _turnIndex = _turnIndex++;
    }
}

//public class LineOfPlay;
