using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Games.Commands.Draw;

public record DrawCommand(GameId GameId, PlayerId PlayerId) : IRequest { }

public class DrawCommandValidator() : AbstractValidator<DrawCommand> { }

public class DrawCommandHandler(ILogger<DrawCommandHandler> logger) : IRequestHandler<DrawCommand>
{
    private readonly ILogger<DrawCommandHandler> _logger = logger;

    public async Task Handle(DrawCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PipAndIvory Command Request: {Command}", request.GetType().Name);
    }
}
