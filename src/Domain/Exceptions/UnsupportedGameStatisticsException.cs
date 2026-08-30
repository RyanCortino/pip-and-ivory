namespace PipAndIvory.Domain.Exceptions;

public class UnsupportedGameStatisticsException : Exception
{
    public UnsupportedGameStatisticsException(int played, int won, int highestScore)
        : base($"Game statistics with played: \"{played}\", won: \"{won}\", highest score: \"{highestScore}\" are unsupported.") { }
}
