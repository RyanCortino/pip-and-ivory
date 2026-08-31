using System.Diagnostics.CodeAnalysis;
using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Players.Commands.UpdatePlayer;

public record UpdatePlayerCommand : IRequest
{
    public PlayerId Id { get; init; }

    public string? DisplayName { get; init; }
}

public class UpdatePlayerCommandValidator : AbstractValidator<UpdatePlayerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePlayerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.DisplayName).NotEmpty().MaximumLength(70);
    }
}

public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Players.FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.DisplayName = request.DisplayName;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
