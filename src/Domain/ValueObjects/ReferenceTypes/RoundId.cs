namespace PipAndIvory.Domain.ValueObjects.ReferenceTypes;

public readonly record struct RoundId(Guid Value)
{
    public static RoundId New() => new(Guid.NewGuid());

    public override string ToString()
    {
        return Value.ToString();
    }
}
