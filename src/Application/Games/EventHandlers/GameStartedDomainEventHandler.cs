using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.Events;

namespace PipAndIvory.Application.Games.EventHandlers;

public class GameStartedDomainEventHandler(ILogger<GameStartedDomainEventHandler> logger)
    : INotificationHandler<GameStartedDomainEvent>
{
    private readonly ILogger<GameStartedDomainEventHandler> _logger = logger;

    public async Task Handle(
        GameStartedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "PipAndIvory Domain Notification: {Event}",
            notification.GetType().Name
        );
    }
}
