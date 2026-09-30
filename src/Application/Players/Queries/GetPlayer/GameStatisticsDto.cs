namespace PipAndIvory.Application.Players.Queries.GetPlayer;

/// <summary>
/// DTO representing aggregated statistics for a particular game (or bucketed period).
/// </summary>
public class GameStatisticsDto
{
    /// <summary>
    /// Total number of times the player has played.
    /// </summary>
    public int Played { get; init; }

    /// <summary>
    /// Total number of wins the player has achieved.
    /// </summary>
    public int Won { get; init; }

    /// <summary>
    /// The highest score the player has achieved in a single play session.
    /// </summary>
    public int HighestScore { get; init; }
}
