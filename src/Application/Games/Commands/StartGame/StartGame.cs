using Microsoft.Extensions.Logging;
using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.Enums;
using PipAndIvory.Domain.Events;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Games.Commands.StartGame;

public record StartGameCommand : IRequest<GameId>
{
    public GameMode GameMode { get; init; } = GameMode.Block;

    public IList<PlayerId> PlayerIds { get; init; } = [];
}

public class StartGameCommandValidator : AbstractValidator<StartGameCommand>
{
    public StartGameCommandValidator()
    {
        RuleFor(v => v.GameMode)
            .NotNull()
            .WithMessage("A game variant must be specified.")
            .IsInEnum()
            .WithMessage("The specified game variant is not supported.");

        RuleFor(v => v.PlayerIds.Count)
            .InclusiveBetween(2, 5)
            .WithMessage("A game must have between 2 and 5 players.");
    }
}

public class StartGameCommandHandler(
    ILogger<StartGameCommandHandler> logger,
    IApplicationDbContext context
) : IRequestHandler<StartGameCommand, GameId>
{
    private readonly ILogger<StartGameCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;

    public async Task<GameId> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PipAndIvory Command Request: {Command}", request.GetType().Name);

        var game = Game.Create(
            request.GameMode is GameMode.Block ? GameVariant.Block : GameVariant.Draw,
            request.PlayerIds
        );

        game.AddDomainEvent(
            new GameStartedDomainEvent(game.Id, [.. game.Participants.Select(g => g.Id)])
        );

        _context.Games.Add(game);

        await _context.SaveChangesAsync(cancellationToken);

        return game.Id;
    }
}
