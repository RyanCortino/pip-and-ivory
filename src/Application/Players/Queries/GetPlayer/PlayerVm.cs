namespace PipAndIvory.Application.Players.Queries.GetPlayer;

/// <summary>
/// View model returned by the <c>GetPlayer</c> query.
/// Contains the requested player's DTO and a collection of aggregated game statistics.
/// </summary>
public class PlayerVm
{
    /// <summary>
    /// The player's data transfer object containing identity and profile fields.
    /// </summary>
    public required PlayerDto Player { get; init; }

    /// <summary>
    /// A read-only collection of aggregated game statistics for this player.
    /// Each element represents a set of metrics (for example, per game type or period).
    /// </summary>
    public IReadOnlyCollection<GameStatisticsDto> GameStatistics { get; init; } = [];
}
