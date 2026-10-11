namespace PipAndIvory.Domain.Entities;

/// <summary>
/// Represents an arm in the game, which consists of one or more branches. Each branch has an open end that can be matched with a tile.
/// </summary>
public class Arm : BaseEntity<int>
{
    public static Arm From(int value, int id)
    {
        var arm = new Arm() { Id = id };

        arm.AddBranch(value);

        return arm;
    }

    public List<Branch> Branches { get; private set; } = [];

    private int _nextBranchId = 0;

    public bool HasMatchingBranch(Bone card) =>
        Branches.Any(branch => branch.HasMatchingOpenEnd(card));

    public void SetCardOnBranch(Bone card, Branch branch)
    {
        // update the existing branch's open end value
        branch.SetTile(card);

        // add additional arm if the card played was a double
        if (card.IsDouble)
            AddBranch(branch.OpenEnd);
    }

    private void AddBranch(int value)
    {
        Branches.Add(Branch.Create(value, _nextBranchId));

        _nextBranchId++;
    }
}
