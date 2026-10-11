namespace PipAndIvory.Domain.Entities;

/// <summary>
/// Represents a branch in the game, which has an open end that can be matched with a tile. Each branch is part of an arm in the tableau.
/// </summary>
public class Branch : BaseEntity<int>
{
    public static Branch Create(int value, int index)
    {
        return new Branch { Id = index, OpenEnd = value };
    }

    public int OpenEnd { get; set; }

    public bool HasMatchingOpenEnd(Bone card) => card.HasFace(OpenEnd);

    public void SetTile(Bone card)
    {
        var exposedValue = card.Pip1 == OpenEnd ? card.Pip2 : card.Pip1;

        OpenEnd = exposedValue;
    }
}
