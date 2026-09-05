using System;
using System.Collections.Generic;
using System.Text;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects;

namespace PipAndIvory.Application.Players.Queries.GetPlayer;

/// <summary>
/// Data transfer object representing a player returned by the GetPlayer query.
/// </summary>
/// <remarks>
/// This DTO is produced by mapping the domain <c>Player</c> entity to a shape suitable for presentation
/// or transport. The nested <c>GameStatisticsDto</c> instances represent per-game-type statistics
/// for the player.
/// </remarks>
public class PlayerDto
{
    /// <summary>
    /// The player's unique identifier, mapped from <c>Player.Id</c> and represented as a string.
    /// </summary>
    public string? Id { get; init; }

    /// <summary>
    /// The player's display name.
    /// </summary>
    public string? DisplayName { get; init; }

    /// <summary>
    /// Aggregated statistics for the player's "block" games.
    /// Mapped from <c>Player.BlockGameStats</c>.
    /// </summary>
    public GameStatisticsDto? BlockGameStats { get; init; }

    /// <summary>
    /// Aggregated statistics for the player's "draw" games.
    /// Mapped from <c>Player.DrawGameStats</c>.
    /// </summary>
    public GameStatisticsDto? DrawGameStats { get; init; }

    /// <summary>
    /// AutoMapper profile used to configure mappings between domain entities and DTOs
    /// used by the GetPlayer query handlers.
    /// </summary>
    private class Mapping : Profile
    {
        /// <summary>
        /// Configures the following mappings:
        /// - <c>Player</c> -> <c>PlayerDto</c> (with <c>Id</c> converted to string and nested game stats mapped)
        /// - <c>GameStatistics</c> -> <c>GameStatisticsDto</c>
        /// </summary>
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
