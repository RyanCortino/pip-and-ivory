using Microsoft.Extensions.Logging;
using PipAndIvory.Application.Common.Interfaces;
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

        gameEntity.StartNewRound();

        await _context.SaveChangesAsync(cancellationToken);
    }
}
