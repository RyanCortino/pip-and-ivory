using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Infrastructure.Data.ValueConverters;

public class PlayerIdConverter : ValueConverter<PlayerId, Guid>
{
    public PlayerIdConverter()
        : base(id => id.Value, value => new PlayerId(value)) { }
}
