using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Events;

public class GameStartedDomainEvent(GameId gameId, IReadOnlyList<PlayerId> players) : BaseEvent
{
    public GameId GameId { get; } = gameId;
    public IReadOnlyList<PlayerId> Players { get; } = players;
}
