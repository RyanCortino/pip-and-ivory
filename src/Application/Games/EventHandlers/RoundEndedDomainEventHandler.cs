using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.Events;

namespace PipAndIvory.Application.Games.EventHandlers;

public class RoundEndedDomainEventHandler(ILogger<RoundEndedDomainEventHandler> logger)
    : INotificationHandler<RoundEndedDomainEvent>
{
    private readonly ILogger<RoundEndedDomainEventHandler> _logger = logger;

    public async Task Handle(
        RoundEndedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "PipAndIvory Domain Notification: {Event}",
            notification.GetType().Name
        );
    }
}
