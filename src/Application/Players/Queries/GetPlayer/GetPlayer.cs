using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Application.Common.Security;

namespace PipAndIvory.Application.Players.Queries.GetPlayer;

[Authorize]
public record GetPlayerQuery : IRequest<PlayerVm>;

public class GetPlayerQueryValidator : AbstractValidator<GetPlayerQuery>
{
    public GetPlayerQueryValidator() { }
}

public class GetPlayerQueryHandler : IRequestHandler<GetPlayerQuery, PlayerVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlayerQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PlayerVm> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        var playerDto = await _context
            .Players.AsNoTracking()
            .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.Null(playerDto, "Player not found");

        return new PlayerVm { Player = playerDto };
    }
}
