using Microsoft.AspNetCore.Http.HttpResults;
using PipAndIvory.Application.Players.Commands.CreatePlayer;

namespace PipAndIvory.Web.Endpoints;

public class Players : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapPost(CreatePlayer);
    }

    [EndpointSummary("Create a new Player")]
    [EndpointDescription(
        "Creates a new player using the provided details and returns the ID of the created player"
    )]
    public static async Task<Created<Guid>> CreatePlayer(
        ISender sender,
        CreatePlayerCommand command
    )
    {
        var playerId = await sender.Send(command); // PlayerId

        return TypedResults.Created($"/{nameof(Players)}/{playerId.Value}", playerId.Value);
    }
}
