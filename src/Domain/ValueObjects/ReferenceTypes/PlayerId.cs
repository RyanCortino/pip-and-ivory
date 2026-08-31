namespace PipAndIvory.Domain.ValueObjects.ReferenceTypes;

public readonly record struct PlayerId(Guid Value)
{
    public static PlayerId New() => new(Guid.NewGuid());

    public override string ToString()
    {
        return Value.ToString();
    }
}
