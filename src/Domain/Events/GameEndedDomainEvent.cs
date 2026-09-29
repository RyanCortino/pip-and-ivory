using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Events;

public class GameEndedDomainEvent(GameId gameId, PlayerId winningPlayerId, Score finalScores)
    : BaseEvent
{
    public GameId GameId { get; } = gameId;
    public PlayerId Winner { get; } = winningPlayerId;
    public Score FinalScores { get; } = finalScores;
}
