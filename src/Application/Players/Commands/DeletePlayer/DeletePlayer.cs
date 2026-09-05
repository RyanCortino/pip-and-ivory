using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Players.Commands.DeletePlayer;

/// <summary>
/// Represents a request to delete a <c>Player</c> identified by a <see cref="PlayerId"/>.
/// </summary>
/// <param name="Id">The identifier of the player to delete.</param>
public record DeletePlayerCommand(PlayerId Id) : IRequest
{
    /// <summary>
    /// Creates a <see cref="DeletePlayerCommand"/> from a <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The GUID that will be wrapped in a <see cref="PlayerId"/>.</param>
    /// <returns>A new <see cref="DeletePlayerCommand"/> for the specified id.</returns>
    public static DeletePlayerCommand From(Guid guid) => new(new PlayerId(guid));
}

/// <summary>
/// Handles <see cref="DeletePlayerCommand"/> instances by removing the corresponding
/// <c>Player</c> entity from the application's database context and persisting the change.
/// </summary>
public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand>
{
    /// <summary>
    /// The application database context used to query and modify player entities.
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePlayerCommandHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public DeletePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the delete command:
    /// - Loads the player entity by <see cref="DeletePlayerCommand.Id"/>.
    /// - Throws if the entity is not found.
    /// - Removes the entity from the context and saves changes.
    /// </summary>
    /// <param name="request">The <see cref="DeletePlayerCommand"/> containing the target player id.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context
            .Players.Where(p => p.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Players.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
