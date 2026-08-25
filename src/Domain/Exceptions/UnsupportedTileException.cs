namespace PipAndIvory.Domain.Exceptions;

public class UnsupportedTileException : Exception
{
    public UnsupportedTileException(string name)
        : base($"Tile \"{name}\" is unsupported.") { }
}
