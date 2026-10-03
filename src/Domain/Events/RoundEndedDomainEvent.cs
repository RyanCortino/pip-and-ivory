using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Events;

public class RoundEndedDomainEvent(
    GameId gameId,
    RoundResult roundResult
//PlayerId? winningPlayerId,
//int pointsAwarded,
//Score cumulativeScores,
//Score roundScores
) : BaseEvent
{
    public GameId GameId { get; } = gameId;
    public RoundResult RoundResult { get; } = roundResult;
    //public PlayerId? Winner { get; } = winningPlayerId;
    //public int PointsAwarded { get; } = pointsAwarded;
    //public Score RoundScores { get; } = roundScores;
    //public Score CumulativeScores { get; } = cumulativeScores;
}
