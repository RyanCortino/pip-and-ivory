using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Players.Commands.CreatePlayer;

/// <summary>
/// Command used to create a new <see cref="Player"/>.
/// Implements <c>IRequest&lt;PlayerId&gt;</c> so it can be handled by MediatR to return the created player's identifier.
/// </summary>
public record CreatePlayerCommand : IRequest<PlayerId>
{
    /// <summary>
    /// Optional display name for the player. Maximum length is validated by <see cref="CreatePlayerCommandValidator"/>.
    /// Leading and trailing whitespace will be trimmed by the handler before persistence.
    /// </summary>
    public string? DisplayName { get; init; }
}

/// <summary>
/// Validates a <see cref="CreatePlayerCommand"/> instance.
/// Currently enforces a maximum length of 70 characters on <see cref="CreatePlayerCommand.DisplayName"/>.
/// </summary>
public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePlayerCommandValidator"/> class
    /// and configures validation rules for the command.
    /// </summary>
    public CreatePlayerCommandValidator()
    {
        RuleFor(v => v.DisplayName).MaximumLength(70);
    }
}

/// <summary>
/// Handles <see cref="CreatePlayerCommand"/> requests by creating a new <see cref="Player"/> entity
/// and saving it to the application database context.
/// </summary>
public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, PlayerId>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Creates a new instance of the handler with the provided application database context.
    /// </summary>
    /// <param name="context">The application database context used to persist the new player.</param>
    public CreatePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the incoming <see cref="CreatePlayerCommand"/>, trims the display name if provided,
    /// persists the new player, and returns the generated <see cref="PlayerId"/>.
    /// </summary>
    /// <param name="request">The command containing the player's data.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The identifier of the newly created player.</returns>
    public async Task<PlayerId> Handle(
        CreatePlayerCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Player { Id = PlayerId.New(), DisplayName = request.DisplayName };

        if (entity.DisplayName is not null)
            entity.DisplayName = entity.DisplayName.Trim();

        _context.Players.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
