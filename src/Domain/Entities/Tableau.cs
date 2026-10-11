using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

/// <summary>
/// Represents the entire layout of the game, including all arms and their branches with open ends.
/// </summary>
public class Tableau : BaseEntity<RoundId>
{
    public static Tableau From(IEnumerable<Bone> stockPile, RoundId roundId)
    {
        var tableau = new Tableau() { Id = roundId, StockPile = [.. stockPile] };

        tableau.ShuffleBoneyard();

        return tableau;
    }

    public IList<Arm> Arms { get; private set; } = [];

    public IList<Hand> PlayerHands { get; private set; } = [];

    public IList<Bone> StockPile { get; private set; } = [];

    public Bone? Spinner { get; private set; }

    private int _nextArmIndex = 0;

    public bool CanPlay(Bone card) => Arms.Any(arm => arm.HasMatchingBranch(card));

    public void ShuffleBoneyard()
    {
        StockPile = [.. StockPile.OrderBy(_ => Guid.NewGuid())];
    }

    public void SetSpinner(Bone bone, int defaultArmsCount = 4)
    {
        Spinner = bone;

        // If the spinner is a double, add the specified number of arms to the
        // tableau, all with the same open end value as the spinner.
        if (bone.IsDouble)
        {
            for (int i = 0; i < defaultArmsCount; i++)
            {
                AddArm(bone.Pip1);
            }

            return;
        }

        // if the spinner is not a double, add two arms to the tableau, one for
        // each pip of the spinner.
        AddArm(bone.Pip1);
        AddArm(bone.Pip2);
    }

    public void SetMatch(Bone bone, Arm arm, Branch branch)
    {
        arm.SetCardOnBranch(bone, branch);
    }

    public Bone DrawTo(Hand hand)
    {
        var drawnBone = StockPile.First();

        StockPile.RemoveAt(0);

        hand.Receive(drawnBone);

        return drawnBone;
    }

    private void AddArm(int value)
    {
        Arms.Add(Arm.From(value, _nextArmIndex));

        _nextArmIndex++;
    }
}
