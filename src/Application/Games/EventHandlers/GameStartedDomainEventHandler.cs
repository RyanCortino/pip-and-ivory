using Microsoft.Extensions.Logging;
using PipAndIvory.Application.Games.Commands.StartRound;
using PipAndIvory.Domain.Events;

namespace PipAndIvory.Application.Games.EventHandlers;

public class GameStartedDomainEventHandler(
    ILogger<GameStartedDomainEventHandler> logger,
    ISender sender
) : INotificationHandler<GameStartedDomainEvent>
{
    private readonly ILogger<GameStartedDomainEventHandler> _logger = logger;
    private readonly ISender _sender = sender;

    public async Task Handle(
        GameStartedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "PipAndIvory Domain Notification: {Event}",
            notification.GetType().Name
        );

        await _sender.Send(
            new StartRoundCommand { GameId = notification.GameId },
            cancellationToken
        );
    }
}
