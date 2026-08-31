using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Players.Commands.DeletePlayer;

public record DeletePlayerCommand(PlayerId Id) : IRequest
{
    public static DeletePlayerCommand From(Guid guid) => new(new PlayerId(guid));
}

public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePlayerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
