using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Events;

public class MoveMadeDomainEvent(GameId gameId, PlayerId player, Move moveDetails) : BaseEvent
{
    public GameId GameId { get; } = gameId;
    public PlayerId Player { get; } = player;
    public Move MoveDetails { get; } = moveDetails;
}
