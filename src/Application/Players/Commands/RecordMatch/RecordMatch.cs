using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Players.Commands.RecordMatch;

public record RecordMatchCommand : IRequest
{
    public PlayerId PlayerId { get; init; }

    public GameVariant Gamemode { get; init; } = GameVariant.Block;

    public bool Won { get; init; }

    public int Score { get; init; }
}

public class RecordMatchCommandValidator : AbstractValidator<RecordMatchCommand>
{
    public RecordMatchCommandValidator() { }
}

public class RecordMatchCommandHandler(IApplicationDbContext context)
    : IRequestHandler<RecordMatchCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(RecordMatchCommand request, CancellationToken cancellationToken)
    {
        var player = await _context.Players.FindAsync([request.PlayerId], cancellationToken);

        Guard.Against.NotFound(request.PlayerId, player);

        player.RecordGameResult(request.Gamemode, request.Won, request.Score);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
