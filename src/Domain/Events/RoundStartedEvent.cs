using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Events;

public class RoundStartedEvent(Round round) : BaseEvent
{
    public Round Round { get; } = round;
}

//public class MoveMadeEvent(Move move) : BaseEvent
//{
//    public Move Move { get; } = move;
//}

public class RoundEndedEvent(Round round) : BaseEvent
{
    public Round Round { get; } = round;
}

public class GameEndedEvent(Game game) : BaseEvent
{
    public Game Game { get; } = game;
}
