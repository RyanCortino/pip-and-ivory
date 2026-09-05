using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Application.Common.Security;

namespace PipAndIvory.Application.Players.Queries.GetPlayer;

/// <summary>
/// Query used to request the current <c>Player</c> view model.
/// </summary>
[Authorize]
public record GetPlayerQuery : IRequest<PlayerVm>;

/// <summary>
/// Handles <see cref="GetPlayerQuery"/> requests.
/// </summary>
/// <remarks>
/// The handler retrieves the first available player from the application's
/// <see cref="IApplicationDbContext.Players"/> set, projects it to a
/// <see cref="PlayerDto"/> using AutoMapper, and returns a <see cref="PlayerVm"/>
/// containing that DTO. The query is secured by the <see cref="AuthorizeAttribute"/>.
/// </remarks>
public class GetPlayerQueryHandler : IRequestHandler<GetPlayerQuery, PlayerVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerQueryHandler"/> class.
    /// </summary>
    /// <param name="context">Application database context used to access player data.</param>
    /// <param name="mapper">AutoMapper instance used to project entities to DTOs.</param>
    public GetPlayerQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the <see cref="GetPlayerQuery"/> by loading the first player, projecting
    /// it to <see cref="PlayerDto"/>, and returning it wrapped in a <see cref="PlayerVm"/>.
    /// </summary>
    /// <param name="request">The query request (no additional properties required).</param>
    /// <param name="cancellationToken">Cancellation token to cancel database operations.</param>
    /// <returns>
    /// A <see cref="PlayerVm"/> containing the projected <see cref="PlayerDto"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when no player is found in the database (via <c>Guard.Against.Null</c>).
    /// </exception>
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
