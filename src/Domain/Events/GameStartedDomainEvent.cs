namespace PipAndIvory.Domain.Events;

public class GameStartedDomainEvent(Game game) : BaseEvent
{
    public Game Game { get; } = game;
}
