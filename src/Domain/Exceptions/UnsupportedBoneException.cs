namespace PipAndIvory.Domain.Exceptions;

public class UnsupportedBoneException : Exception
{
    public UnsupportedBoneException(string name)
        : base($"Tile \"{name}\" is unsupported.") { }
}
