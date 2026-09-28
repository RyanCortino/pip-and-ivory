using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Participant : BaseEntity<PlayerId>
{
    public int CurrentScore { get; set; }

    public bool IsWinner { get; set; }
}
