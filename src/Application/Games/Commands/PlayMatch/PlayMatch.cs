using Microsoft.Extensions.Logging;

namespace PipAndIvory.Application.Games.Commands.PlayMatch;

public record PlayMatchCommand : IRequest { }

public class PlayMatchCommandValidator : AbstractValidator<PlayMatchCommand> { }

public class PlayMatchCommandHandler(ILogger<PlayMatchCommandHandler> logger)
    : IRequestHandler<PlayMatchCommand>
{
    private readonly ILogger<PlayMatchCommandHandler> _logger = logger;

    public async Task Handle(PlayMatchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PipAndIvory Command Request: {Command}", request.GetType().Name);
    }
}
