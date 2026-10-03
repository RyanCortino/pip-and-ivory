namespace PipAndIvory.Domain.Enums;

public enum RoundEndReason
{
    Domino = 0, // The round ended because a player played their last tile.

    Blocked = 1, // The round ended because no player could make a valid move.

    Draw = 2,
}
