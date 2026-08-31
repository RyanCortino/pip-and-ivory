using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Players.Commands.CreatePlayer;

public record CreatePlayerCommand : IRequest<PlayerId>
{
    public string? DisplayName { get; init; }
}

public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidator()
    {
        RuleFor(v => v.DisplayName).MaximumLength(70);
    }
}

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, PlayerId>
{
    private readonly IApplicationDbContext _context;

    public CreatePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
