namespace PipAndIvory.Domain.ValueObjects.ReferenceTypes;

public abstract class ReferenceTypeId<T>(Guid value) : ValueObject
    where T : ReferenceTypeId<T>
{
    public Guid Value { get; private set; } = value;

    public static T From(Guid value)
    {
        var referenceId = (T)Activator.CreateInstance(typeof(T), value)!;

        // self-validation logic can be added here if needed

        return referenceId;
    }

    public static T New => From(Guid.NewGuid());

    public static implicit operator Guid(ReferenceTypeId<T> id)
    {
        return id.Value;
    }

    // Explicit conversion from Guid (returns derived type)
    public static explicit operator ReferenceTypeId<T>(Guid value)
    {
        return From(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
