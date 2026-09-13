using Microsoft.AspNetCore.Http.HttpResults;
using PipAndIvory.Application.Games.Commands.StartGame;

namespace PipAndIvory.Web.Endpoints;

public class Games : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapPost(StartGame);
    }

    [EndpointSummary("Start a new Game")]
    [EndpointDescription("Starts a new game with the specified players and game variant")]
    public static async Task<Created<Guid>> StartGame(ISender sender, StartGameCommand command)
    {
        var gameId = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Games)}/{gameId.Value}", gameId.Value);
    }
}
