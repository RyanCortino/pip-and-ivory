using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Move(PlayerId playerId, MoveType moveType, int sequenceIndex) : ValueObject
{
    public static Move From(
        PlayerId playerId,
        MoveType moveType,
        Bone? tile,
        Branch? branch,
        int sequence
    )
    {
        return new Move(playerId, moveType, sequence) { Tile = tile, BranchId = branch!.Id };
    }

    public PlayerId PlayerId { get; private set; } = playerId;

    public MoveType MoveType { get; private set; } = moveType;

    public int SequenceIndex { get; private set; } = sequenceIndex;

    public Bone? Tile { get; private set; }

    public int BranchId { get; private set; }

    public static Move SetSpinner(PlayerId playerId, Bone spinner, int sequence) =>
        From(playerId, MoveType.SetSpinner, spinner, null, sequence);

    public static Move PlayMatch(PlayerId playerId, Bone tile, Branch branch, int sequence) =>
        From(playerId, MoveType.PlayMatch, tile, branch, sequence);

    public static Move Draw(PlayerId playerId, Bone drawnTile, int sequence) =>
        From(playerId, MoveType.Draw, drawnTile, null, sequence);

    public static Move Pass(PlayerId playerId, int sequence) =>
        From(playerId, MoveType.Pass, null, null, sequence);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PlayerId;
        yield return MoveType;
        yield return SequenceIndex;
    }
}
