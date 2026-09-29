using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.Events;

namespace PipAndIvory.Application.Games.EventHandlers;

public class RoundStartedDomainEventHandler(ILogger<GameStartedDomainEventHandler> logger)
    : INotificationHandler<RoundStartedDomainEvent>
{
    private readonly ILogger<GameStartedDomainEventHandler> _logger = logger;

    public async Task Handle(
        RoundStartedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "PipAndIvory Domain Notification: {Event}",
            notification.GetType().Name
        );
    }
}
