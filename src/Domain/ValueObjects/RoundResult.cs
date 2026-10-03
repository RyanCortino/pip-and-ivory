namespace PipAndIvory.Domain.ValueObjects;

public class RoundResult(RoundEndReason endReason, Score score) : ValueObject
{
    public RoundEndReason EndReason { get; } = endReason;

    public Score Score { get; } = score;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return EndReason;
        yield return Score;
    }
}
