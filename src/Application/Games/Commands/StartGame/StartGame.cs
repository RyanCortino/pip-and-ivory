using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.Enums;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Games.Commands.StartGame;

public record StartGameCommand : IRequest<GameId>
{
    public GameModes GameMode { get; init; } = GameModes.Block;

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

public class StartGameCommandHandler : IRequestHandler<StartGameCommand, GameId>
{
    private readonly IApplicationDbContext _context;

    public StartGameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GameId> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        var entity = Game.Start(
            request.GameMode is GameModes.Block ? GameVariant.Block : GameVariant.Draw,
            request.PlayerIds
        );

        _context.Games.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
