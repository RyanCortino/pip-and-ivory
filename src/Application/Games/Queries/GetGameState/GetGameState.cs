using PipAndIvory.Application.Common.Interfaces;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.Games.Queries.GetGameState;

public record GetGameStateQuery(GameId GameId) : IRequest<GamesVm>;

public class GetGameStateQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetGameStateQuery, GamesVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<GamesVm> Handle(
        GetGameStateQuery request,
        CancellationToken cancellationToken
    )
    {
        var gameState = await _context
            .Games.AsNoTracking()
            .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        Guard.Against.Null(gameState, "Game not found");

        return new GamesVm { GameState = gameState };
    }
}

public class GamesVm
{
    public required GameDto GameState { get; init; }

    public IReadOnlyCollection<ParticipantDto> Participants { get; init; } = [];

    public IReadOnlyCollection<RoundDto> Rounds { get; init; } = [];
}

public class GameDto
{
    public string? Id { get; init; }
    public string? GameVariant { get; init; }
    public int GameStatus { get; init; }
    public IReadOnlyList<ParticipantDto> Participants { get; init; } = [];
    public RoundDto? CurrentRound { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Game, GameDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()))
                .ForMember(d => d.GameVariant, opt => opt.MapFrom(s => s.GameVariant.ToString()))
                .ForMember(d => d.GameStatus, opt => opt.MapFrom(s => (int)s.GameStatus))
                .ForMember(d => d.Participants, opt => opt.MapFrom(s => s.Participants))
                .ForMember(d => d.CurrentRound, opt => opt.MapFrom(s => s.CurrentRound));

            CreateMap<Participant, ParticipantDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(p => p.Id.ToString()));

            CreateMap<Round, RoundDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(r => r.Id.ToString()))
                .ForMember(
                    d => d.CurrentTurnPlayerId,
                    opt => opt.MapFrom(r => r.CurrentTurn.ToString())
                )
                .ForMember(d => d.PlayerHands, opt => opt.MapFrom(r => r.PlayerHands));

            CreateMap<Hand, HandDto>();

            CreateMap<Bone, BoneDto>()
                .ForMember(d => d.FirstPipValue, opt => opt.MapFrom(b => b.Pip1))
                .ForMember(d => d.SecondPipValue, opt => opt.MapFrom(b => b.Pip2));
        }
    }
}

public class ParticipantDto
{
    public string? Id { get; init; }
    public int CurrentScore { get; init; }
    public bool IsWinner { get; init; }
}

public class RoundDto
{
    public string? Id { get; init; }
    public string? CurrentTurnPlayerId { get; init; }
    public IReadOnlyList<HandDto> PlayerHands { get; init; } = [];
}

public class HandDto
{
    public IReadOnlyList<BoneDto> Bones { get; init; } = [];
}

public class BoneDto
{
    public int? FirstPipValue { get; init; }
    public int? SecondPipValue { get; init; }
}
