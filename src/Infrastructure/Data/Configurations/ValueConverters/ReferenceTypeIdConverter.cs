using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Infrastructure.Data.Configurations.ValueConverters;

public class ReferenceTypeIdConverter<T> : ValueConverter<T, Guid>
    where T : ReferenceTypeId<T>
{
    public ReferenceTypeIdConverter()
        : base(
            modelValue => modelValue.Value,
            providerValue => (T)(ReferenceTypeId<T>)providerValue
        ) { }
}
