using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Games.Commands.Pass;

public record PassCommand(GameId GameId, PlayerId PlayerId) : IRequest { }

public class PassCommandValidator : AbstractValidator<PassCommand> { }

public class PassCommandHandler(ILogger<PassCommandHandler> logger) : IRequestHandler<PassCommand>
{
    private readonly ILogger<PassCommandHandler> _logger = logger;

    public async Task Handle(PassCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PipAndIvory Command Request: {Command}", request.GetType().Name);
    }
}
