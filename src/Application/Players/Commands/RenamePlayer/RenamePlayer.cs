using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Players.Commands.RenamePlayer;

/// <summary>
/// Command to update an existing player's mutable fields.
/// </summary>
/// <remarks>
/// This command is intended to be handled by <see cref="RenamePlayerCommandHandler"/>.
/// Only fields that are present on the command will be applied to the entity (in this file,
/// only <see cref="DisplayName"/> is mutable).
/// </remarks>
public record RenamePlayerCommand : IRequest
{
    /// <summary>
    /// The identifier of the player to update.
    /// </summary>
    public PlayerId Id { get; init; }

    /// <summary>
    /// The new display name for the player. May be <c>null</c> if no change is requested,
    /// but when provided it must satisfy validation rules enforced by
    /// <see cref="RenamePlayerCommandValidator"/>.
    /// </summary>
    public string? DisplayName { get; init; }
}

/// <summary>
/// Validates instances of <see cref="RenamePlayerCommand"/>.
/// </summary>
/// <remarks>
/// Uses FluentValidation to enforce command-level invariants. Additional domain checks
/// (for example ensuring uniqueness) can be added here if required.
/// </remarks>
public class RenamePlayerCommandValidator : AbstractValidator<RenamePlayerCommand>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Creates a new validator instance.
    /// </summary>
    /// <param name="context">The application database context. Provided so rules can use the DB if needed.</param>
    public RenamePlayerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        // Ensure a non-empty display name when provided and constrain its length.
        RuleFor(v => v.DisplayName).NotEmpty().MaximumLength(70);
    }
}

/// <summary>
/// Handles <see cref="RenamePlayerCommand"/> messages.
/// </summary>
/// <remarks>
/// Responsibilities:
/// - Load the player entity from the database.
/// - Verify the player exists (throws when not found).
/// - Apply updates from the command to the entity.
/// - Persist changes to the database.
/// </remarks>
public class RenamePlayerCommandHandler : IRequestHandler<RenamePlayerCommand>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Constructs the handler.
    /// </summary>
    /// <param name="context">The application database context used to query and persist players.</param>
    public RenamePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Processes the update command: fetches the target player, applies the requested changes,
    /// and saves the updated entity to the database.
    /// </summary>
    /// <param name="request">The update command containing the target player id and updated data.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <exception cref="Exception">Thrown by guard if the target player is not found.</exception>
    public async Task Handle(RenamePlayerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Players.FindAsync([request.Id], cancellationToken);

        // Guard.Against.NotFound will throw a domain-specific exception if the entity is null.
        Guard.Against.NotFound(request.Id, entity);

        // Apply changes from the command to the entity.
        entity.DisplayName = request.DisplayName;

        // Persist changes.
        await _context.SaveChangesAsync(cancellationToken);
    }
}
