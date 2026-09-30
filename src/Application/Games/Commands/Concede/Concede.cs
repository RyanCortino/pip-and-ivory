using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Games.Commands.Concede;

public record ConcedeGameCommand(GameId GameId, PlayerId PlayerId) : IRequest { }

public class ConcedeGameCommandValidator : AbstractValidator<ConcedeGameCommand> { }

public class ConcedeGameCommandHandler(ILogger<ConcedeGameCommandHandler> logger)
    : IRequestHandler<ConcedeGameCommand>
{
    private readonly ILogger<ConcedeGameCommandHandler> _logger = logger;

    public async Task Handle(ConcedeGameCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PipAndIvory Command Request: {Command}", request.GetType().Name);
    }
}
