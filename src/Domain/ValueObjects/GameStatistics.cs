using System;
using System.Collections.Generic;
using System.Text;

namespace PipAndIvory.Domain.ValueObjects;

/// <summary>
/// Represents aggregated game statistics for a player: number of games played, number won, and highest score.
/// </summary>
/// <param name="played">Total number of games played (must be &gt;= 0).</param>
/// <param name="won">Total number of games won (must be &gt;= 0 and &lt;= <paramref name="played"/>).</param>
/// <param name="highestScore">Highest score achieved across games (must be &gt;= 0).</param>
public class GameStatistics(int played, int won, int highestScore) : ValueObject
{
    /// <summary>
    /// Creates a validated <see cref="GameStatistics"/> instance.
    /// </summary>
    /// <remarks>
    /// Validates that all supplied values are non-negative and that <paramref name="won"/> is not greater than <paramref name="played"/>.
    /// Use this factory method to ensure invariants rather than calling the primary constructor directly.
    /// </remarks>
    /// <param name="played">Total games played.</param>
    /// <param name="won">Total games won.</param>
    /// <param name="highestScore">Highest score.</param>
    /// <returns>A new <see cref="GameStatistics"/> instance.</returns>
    /// <exception cref="UnsupportedGameStatisticsException">
    /// Thrown when any value is negative or when <paramref name="won"/> is greater than <paramref name="played"/>.
    /// </exception>
    public static GameStatistics From(int played, int won, int highestScore)
    {
        var gameStatistics = new GameStatistics(played, won, highestScore);

        if (gameStatistics.Played < 0 || gameStatistics.Won < 0 || gameStatistics.HighestScore < 0)
        {
            throw new UnsupportedGameStatisticsException(played, won, highestScore);
        }

        if (gameStatistics.Won > gameStatistics.Played)
        {
            throw new UnsupportedGameStatisticsException(played, won, highestScore);
        }

        return gameStatistics;
    }

    /// <summary>
    /// Number of games played.
    /// </summary>
    public int Played { get; private set; } = played;

    /// <summary>
    /// Number of games won.
    /// </summary>
    public int Won { get; private set; } = won;

    /// <summary>
    /// Highest score achieved across all recorded games.
    /// </summary>
    public int HighestScore { get; private set; } = highestScore;

    /// <summary>
    /// An empty/default statistics instance (0 played, 0 won, 0 highest score).
    /// </summary>
    public static GameStatistics None => new(0, 0, 0);

    /// <summary>
    /// Returns a new <see cref="GameStatistics"/> with a recorded game applied.
    /// </summary>
    /// <param name="won">Whether the recorded game was a win.</param>
    /// <param name="score">Score achieved in the recorded game.</param>
    /// <returns>A new <see cref="GameStatistics"/> with updated counters and highest score.</returns>
    public GameStatistics RecordGame(bool won, int score) =>
        From(Played + 1, Won + (won ? 1 : 0), Math.Max(HighestScore, score));

    /// <summary>
    /// Provides the sequence of components that define equality for this value object.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Played;
        yield return Won;
        yield return HighestScore;
    }
}
