using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

/// <summary>
/// Represents a player in the domain model.
/// Inherits auditable properties from <see cref="BaseAuditableEntity{TKey}"/> using <see cref="PlayerId"/> as the key.
/// </summary>
public class Player : BaseAuditableEntity<PlayerId>
{
    /// <summary>
    /// The player's display name. May be <see langword="null"/> when not set.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Aggregated game statistics for the 'Block' game variant.
    /// Initialized to <see cref="GameStatistics.None"/> and updated via <see cref="RecordGameResult(GameVariant, bool, int)"/>.
    /// </summary>
    public GameStatistics BlockGameStats { get; private set; } = GameStatistics.None;

    /// <summary>
    /// Aggregated game statistics for the 'Draw' game variant (and other non-'Block' variants).
    /// Initialized to <see cref="GameStatistics.None"/> and updated via <see cref="RecordGameResult(GameVariant, bool, int)"/>.
    /// </summary>
    public GameStatistics DrawGameStats { get; private set; } = GameStatistics.None;

    public void Rename(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            name = GenerateFriendlyName();

        DisplayName = name.Trim();
    }

    /// <summary>
    /// Records the outcome of a finished game and updates the appropriate per-variant statistics.
    /// </summary>
    /// <param name="gamemode">
    /// The game variant played. If <paramref name="gamemode"/> equals <see cref="GameVariant.Block"/>,
    /// the result is applied to <see cref="BlockGameStats"/>; otherwise it is applied to <see cref="DrawGameStats"/>.
    /// </param>
    /// <param name="won">Whether the player won the recorded game.</param>
    /// <param name="score">The score achieved in the recorded game.</param>
    /// <remarks>
    /// This method delegates to <see cref="GameStatistics.RecordGame(bool, int)"/>, which returns a new
    /// <see cref="GameStatistics"/> instance and enforces validation (for example, non-negative scores).
    /// </remarks>
    /// <exception cref="UnsupportedGameStatisticsException">
    /// Thrown by <see cref="GameStatistics"/> factory methods if the supplied values would result in invalid statistics
    /// (for example, a negative <paramref name="score"/>).
    /// </exception>
    public void RecordGameResult(GameVariant gamemode, bool won, int score)
    {
        if (gamemode != GameVariant.Block)
        {
            DrawGameStats = DrawGameStats.RecordGame(won, score);
            return;
        }

        BlockGameStats = BlockGameStats.RecordGame(won, score);
    }

    private string GenerateFriendlyName()
    {
        string[] adjectives =
        [
            "Sneaky",
            "Swift",
            "Mighty",
            "Frosty",
            "Golden",
            "Shadow",
            "Cosmic",
            "Cranky",
            "Brave",
            "Lively",
        ];

        string[] nouns =
        [
            "Panda",
            "Raptor",
            "Knight",
            "Wizard",
            "Badger",
            "Falcon",
            "Coyote",
            "Ninja",
            "Goblin",
            "Phoenix",
        ];

        // Extract the raw 16 bytes from the GUID
        byte[] bytes = Id.Value.ToByteArray();

        // Convert chunks of bytes into integers using BitConverter
        // BitConverter.ToInt32 requires 4 bytes
        int adjIndex = Math.Abs(BitConverter.ToInt32(bytes, 0));
        int nounIndex = Math.Abs(BitConverter.ToInt32(bytes, 4));
        int suffixNumber = Math.Abs(BitConverter.ToInt32(bytes, 8));

        // Use modulo (%) to map the large integers to your list sizes
        string selectedAdj = adjectives[adjIndex % adjectives.Length];
        string selectedNoun = nouns[nounIndex % nouns.Length];

        // Create a 4 - digit numeric suffix(0000 to 9999) for extra safety
        int finalSuffix = suffixNumber % 10000;

        // Combine them into a display-friendly name
        return $"{selectedAdj}{selectedNoun}#{finalSuffix:D4}";
    }
}
