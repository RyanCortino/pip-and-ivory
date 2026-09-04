using System;
using System.Collections.Generic;
using System.Text;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects;

namespace PipAndIvory.Application.Players.Queries.GetPlayer;

public class PlayerDto
{
    public string? Id { get; init; }

    public string? DisplayName { get; init; }

    public GameStatisticsDto? BlockGameStats { get; init; }

    public GameStatisticsDto? DrawGameStats { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Player, PlayerDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()))
                .ForMember(d => d.BlockGameStats, opt => opt.MapFrom(s => s.BlockGameStats))
                .ForMember(d => d.DrawGameStats, opt => opt.MapFrom(s => s.DrawGameStats));

            CreateMap<GameStatistics, GameStatisticsDto>();
        }
    }
}
