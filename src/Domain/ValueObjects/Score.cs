using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.ValueObjects;

public class Score(IReadOnlyDictionary<PlayerId, int> playerScores) : ValueObject
{
    public IReadOnlyDictionary<PlayerId, int> PlayerScores { get; } = playerScores;

    public PlayerId Leader => PlayerScores.MaxBy(x => x.Value).Key;

    public int LeaderScore => PlayerScores.Values.Max();

    public int GetScoreFor(PlayerId playerId)
    {
        return PlayerScores.TryGetValue(playerId, out var score) ? score : 0;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PlayerScores;
    }
}
