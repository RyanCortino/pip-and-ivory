using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Player : BaseAuditableEntity<PlayerId>
{
    public string? DisplayName { get; set; }

    public GameStatistics BlockGameStats { get; private set; } = GameStatistics.None;

    public GameStatistics DrawGameStats { get; private set; } = GameStatistics.None;

    public void RecordGameResult(GameVariant gamemode, bool won, int score)
    {
        if (gamemode != GameVariant.Block)
        {
            DrawGameStats = DrawGameStats.RecordGame(won, score);
            return;
        }

        BlockGameStats = BlockGameStats.RecordGame(won, score);
    }
}
