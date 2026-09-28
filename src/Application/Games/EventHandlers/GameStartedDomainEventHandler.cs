using Microsoft.Extensions.Logging;
using PipAndIvory.Domain.Events;

namespace PipAndIvory.Application.Games.EventHandlers;

public class GameStartedDomainEventHandler(
    ILogger<GameStartedDomainEventHandler> logger,
    IMediator mediator
) : INotificationHandler<GameStartedDomainEvent>
{
    private readonly ILogger<GameStartedDomainEventHandler> _logger = logger;

    private readonly IMediator _mediator = mediator;

    public async Task Handle(
        GameStartedDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "PipAndIvory Domain Event: {DomainEvent}",
            notification.GetType().Name
        );

        //await _mediator.Send(
        //    new SetupGameCommand { GameId = notification.Game.Id },
        //    cancellationToken
        //);
    }
}
