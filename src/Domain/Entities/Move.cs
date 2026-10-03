using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Move : BaseEntity<int>
{
    public static Move Create(
        PlayerId playerId,
        MoveType moveType,
        Bone? tile,
        BranchId? branchId,
        int sequence
    )
    {
        return new Move()
        {
            Id = sequence,
            PlayerId = playerId,
            Type = moveType,
            Tile = tile,
            BranchId = branchId,
        };
    }

    public static Move PlayMatch(PlayerId playerId, Bone tile, BranchId branchId, int sequence) =>
        Create(playerId, MoveType.PlayMatch, tile, branchId, sequence);

    public static Move Draw(PlayerId playerId, Bone tile, int sequence) =>
        Create(playerId, MoveType.Draw, tile, null, sequence);

    public static Move Pass(PlayerId playerId, int sequence) =>
        Create(playerId, MoveType.Pass, null, null, sequence);

    public PlayerId PlayerId { get; private set; } = null!;

    public MoveType Type { get; private set; }

    public Bone? Tile { get; private set; }

    public BranchId? BranchId { get; private set; }
}

public class OpenEnd(int value) : BaseEntity<BranchId>
{
    public int Value { get; } = value;
}
