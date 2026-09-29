using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Events;

public class RoundEndedDomainEvent(
    GameId gameId,
    PlayerId? winningPlayerId,
    int pointsAwarded,
    Score roundScores,
    Score cumulativeScores
) : BaseEvent
{
    public GameId GameId { get; } = gameId;
    public PlayerId? Winner { get; } = winningPlayerId;
    public int PointsAwarded { get; } = pointsAwarded;
    public Score RoundScores { get; } = roundScores;
    public Score CumulativeScores { get; } = cumulativeScores;
}
