namespace PipAndIvory.Application.Players.Queries.GetPlayer;

public class PlayerVm
{
    public required PlayerDto Player { get; init; }

    public IReadOnlyCollection<GameStatisticsDto> GameStatistics { get; init; } = [];
}

public class GameStatisticsDto
{
    public int Played { get; init; }

    public int Won { get; init; }

    public int HighestScore { get; init; }
}
