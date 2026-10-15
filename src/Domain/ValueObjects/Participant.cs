using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.ValueObjects;

public class Participant(PlayerId playerId, int score) : ValueObject
{
    public static Participant From(PlayerId playerId, int score = 0)
    {
        return new Participant(playerId, score);
    }

    public PlayerId PlayerId { get; private set; } = playerId;

    public int Score { get; private set; } = score;

    public bool IsWinner { get; private set; } = false;

    public Participant WithScore(int newScore)
    {
        return new Participant(PlayerId, newScore);
    }

    public Participant AsWinner()
    {
        return new Participant(PlayerId, Score) { IsWinner = true };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PlayerId;
    }
}
