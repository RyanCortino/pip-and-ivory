using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Hand : BaseEntity<PlayerId>
{
    public List<Bone> Bones { get; set; } = new List<Bone>();
}
