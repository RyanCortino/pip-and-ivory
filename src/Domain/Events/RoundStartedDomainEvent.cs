using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Events;

public class RoundStartedDomainEvent(GameId gameId, RoundId roundId, PlayerId startingPlayer)
    : BaseEvent
{
    public GameId GameId { get; } = gameId;
    public RoundId RoundId { get; } = roundId;
    public PlayerId StartingPlayer { get; } = startingPlayer;
}
