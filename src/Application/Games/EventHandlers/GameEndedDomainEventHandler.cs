using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.Events;

namespace PipAndIvory.Application.Games.EventHandlers;

public class GameEndedDomainEventHandler(ILogger<GameEndedDomainEventHandler> logger)
    : INotificationHandler<GameEndedDomainEvent>
{
    private readonly ILogger<GameEndedDomainEventHandler> _logger = logger;

    public async Task Handle(GameEndedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "PipAndIvory Domain Notification: {Event}",
            notification.GetType().Name
        );
    }
}
