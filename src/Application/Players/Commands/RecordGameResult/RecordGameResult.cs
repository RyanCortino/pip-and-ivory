using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Players.Commands.RecordGameResult;

public record RecordGameResultCommand : IRequest
{
    public PlayerId PlayerId { get; init; }

    public GameVariant Gamemode { get; init; } = GameVariant.Block;

    public bool Won { get; init; }

    public int Score { get; init; }
}

public class RecordGameResultCommandValidator : AbstractValidator<RecordGameResultCommand>
{
    public RecordGameResultCommandValidator()
    {
        RuleFor(x => x.PlayerId)
            .Must(pid => pid.Value != Guid.Empty)
            .WithMessage("A valid PlayerId must be provided.");

        RuleFor(x => x.Gamemode)
            .NotNull()
            .WithMessage("A game variant must be specified.")
            .Must(g => GameVariant.SupportedVariants.Contains(g))
            .WithMessage("The specified game variant is not supported.");

        RuleFor(x => x.Score).GreaterThanOrEqualTo(0).WithMessage("Score must be non-negative.");
    }
}

public class RecordMatchCommandHandler(IApplicationDbContext context)
    : IRequestHandler<RecordGameResultCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(RecordGameResultCommand request, CancellationToken cancellationToken)
    {
        var player = await _context.Players.FindAsync([request.PlayerId], cancellationToken);

        Guard.Against.NotFound(request.PlayerId, player);

        player.RecordGameResult(request.Gamemode, request.Won, request.Score);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
