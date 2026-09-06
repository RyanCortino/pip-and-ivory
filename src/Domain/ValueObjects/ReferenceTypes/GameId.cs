namespace PipAndIvory.Domain.ValueObjects.ReferenceTypes;

public readonly record struct GameId(Guid Value)
{
    public static GameId New() => new(Guid.NewGuid());

    public override string ToString()
    {
        return Value.ToString();
    }
}
