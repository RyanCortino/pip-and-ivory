using Microsoft.Extensions.Logging;
using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Games.Commands.Pass;

public record PassCommand(GameId GameId, PlayerId PlayerId) : IRequest { }

public class PassCommandValidator : AbstractValidator<PassCommand> { }

public class PassCommandHandler(ILogger<PassCommandHandler> logger, IApplicationDbContext context)
    : IRequestHandler<PassCommand>
{
    private readonly ILogger<PassCommandHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(PassCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PipAndIvory Command Request: {Command}", request.GetType().Name);

        var game = await _context
            .Games.Include(g => g.Rounds)
            .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

        Guard.Against.Null(game, "Game Not Found");

        game.Pass(request.PlayerId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
