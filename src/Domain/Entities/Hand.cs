using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Domain.Entities;

public class Hand : BaseEntity<PlayerId>
{
    public static Hand From(PlayerId playerId, IEnumerable<Bone> startingHand)
    {
        var hand = new Hand() { Id = playerId };

        foreach (var card in startingHand)
        {
            hand.Receive(card);
        }

        return hand;
    }

    public IReadOnlyList<Bone> Bones => _bones.AsReadOnly();
    private readonly List<Bone> _bones = [];

    public void Receive(Bone card)
    {
        _bones.Add(card);
    }

    public void Release(Bone card)
    {
        _bones.Remove(card);
    }
}
