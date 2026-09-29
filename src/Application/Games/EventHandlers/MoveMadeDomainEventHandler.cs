using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.Events;

namespace PipAndIvory.Application.Games.EventHandlers;

public class MoveMadeDomainEventHandler(ILogger<MoveMadeDomainEventHandler> logger)
    : INotificationHandler<MoveMadeDomainEvent>
{
    private readonly ILogger<MoveMadeDomainEventHandler> _logger = logger;

    public async Task Handle(MoveMadeDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "PipAndIvory Domain Notification: {Event}",
            notification.GetType().Name
        );
    }
}
