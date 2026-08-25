namespace PipAndIvory.Domain.Exceptions;

public class UnsupportedCardinalDirectionException : Exception
{
    public UnsupportedCardinalDirectionException(string code)
        : base($"Cardinal direction \"{code}\" is unsupported.") { }
}
