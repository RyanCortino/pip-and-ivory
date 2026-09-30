using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Infrastructure.Data.Configurations.ValueConverters;

public class ReferenceTypeIdConverter<T> : ValueConverter<ReferenceTypeId<T>, Guid>
    where T : ReferenceTypeId<T>
{
    public ReferenceTypeIdConverter()
        : base(id => id.Value, value => (T)(ReferenceTypeId<T>)value) { }
}
