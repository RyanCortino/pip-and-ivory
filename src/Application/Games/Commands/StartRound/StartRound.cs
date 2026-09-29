using Microsoft.Extensions.Logging;
using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.Events;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Games.Commands.StartRound;

public record StartRoundCommand : IRequest
{
    public GameId GameId { get; init; } = default!;
}

public class StartRoundCommandHandler(
    ILogger<StartRoundCommandHandler> logger,
    IApplicationDbContext context
) : IRequestHandler<StartRoundCommand>
{
    private readonly ILogger<StartRoundCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(StartRoundCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PipAndIvory Command Request: {Command}", request.GetType().Name);

        var gameEntity = await _context.Games.FindAsync([request.GameId], cancellationToken);

        Guard.Against.NotFound(nameof(gameEntity), gameEntity);

        // Create a new round
        var round = new Round
        {
            Id = RoundId.New,
            Boneyard = ShuffledNewDeck(), // Set the bonyard to a new shuffled deck of bones
        };

        // Deal hands to each player, removing bones from the boneyard as they're dealt
        var hands = DealPlayerHands(round.Id, gameEntity.Participants, round.Boneyard);

        //round.PlayerHands.AddRange(hands);

        // Determine the turn order based on the highest weight first bone in each player's hand
        List<PlayerId> turnOrder =
        [
            .. hands.OrderByDescending(h => h.Bones.First().Weight).Select(h => h.Id),
        ];

        //round.TurnOrder.AddRange(turnOrder);

        //Add the new round to the game entity
        round.AddDomainEvent(
            new RoundStartedDomainEvent(gameEntity.Id, round.Id, turnOrder.First())
        );

        gameEntity.Rounds.Add(round);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static List<Hand> DealPlayerHands(
        RoundId roundId,
        IList<Participant> participants,
        List<Bone> boneyard
    )
    {
        var handSize = participants.Count <= 3 ? 7 : 5;

        var hands = new List<Hand>();

        // Assuming you have access to the game entity and its participants
        foreach (var participant in participants)
        {
            List<Bone> bones = [.. boneyard.Take(handSize)];

            hands.Add(
                new Hand
                {
                    Id = participant.Id,
                    RoundId = roundId,
                    Bones = bones,
                }
            );

            boneyard.RemoveRange(0, handSize);
        }

        return hands;
    }

    private static List<Bone> ShuffledNewDeck()
    {
        return [.. Bone.SupportedBones.OrderBy(_ => Guid.NewGuid())];
    }
}
