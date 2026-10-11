using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.ValueObjects;

public class RoundParticipant(PlayerId playerId, int initialScore) : ValueObject
{
    public static RoundParticipant From(PlayerId playerId, int initialScore = 0)
    {
        return new RoundParticipant(playerId, initialScore);
    }

    public PlayerId PlayerId { get; private set; } = playerId;

    public int Score { get; private set; } = initialScore;

    public bool IsWinner { get; private set; } = false;

    public RoundParticipant WithScore(int newScore)
    {
        return new RoundParticipant(PlayerId, newScore);
    }

    public RoundParticipant AsWinner()
    {
        return new RoundParticipant(PlayerId, Score) { IsWinner = true };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PlayerId;
    }
}
