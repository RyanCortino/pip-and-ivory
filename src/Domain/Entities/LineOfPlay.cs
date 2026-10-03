using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class LineOfPlay : BaseEntity<RoundId>
{
    private readonly List<OpenEnd> _openEnds = [];

    public bool IsEmpty => _openEnds.Count == 0;

    public IReadOnlyList<OpenEnd> OpenEnds => _openEnds;

    public bool CanPlay(Bone tile) => IsEmpty || _openEnds.Any(end => tile.HasFace(end.Value));

    public void Place(Bone tile, BranchId branchId)
    {
        if (IsEmpty)
        {
            var first = BranchId.New;
            var second = BranchId.New;
            _openEnds.Add(new OpenEnd(tile.Pip1) { Id = first });
            _openEnds.Add(new OpenEnd(tile.Pip2) { Id = second });
            return;
        }

        var end =
            _openEnds.SingleOrDefault(e => e.Id == branchId)
            ?? throw new InvalidOperationException("No open branch with that id.");

        if (!tile.HasFace(end.Value))
            throw new InvalidOperationException($"{tile} does not match branch {branchId}.");

        var exposedValue = tile.Pip1 == end.Value ? tile.Pip2 : tile.Pip1;

        _openEnds.Remove(end);

        _openEnds.Add(new OpenEnd(exposedValue) { Id = branchId });

        // TODO: Implement rule for doubles. If the tile is a double, it should
        // create a new open end with the same value as the double. This is commented out for
        // now, but can be implemented later.
        //  if (tile.IsDouble)
        //      _openEnds.Add(new OpenEnd(exposedValue) { Id = BranchId.New });
    }
}
