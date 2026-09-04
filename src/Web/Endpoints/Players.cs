using Microsoft.AspNetCore.Http.HttpResults;
using PipAndIvory.Application.Players.Commands.CreatePlayer;
using PipAndIvory.Application.Players.Commands.DeletePlayer;
using PipAndIvory.Application.Players.Commands.UpdatePlayer;
using PipAndIvory.Application.Players.Queries.GetPlayer;

namespace PipAndIvory.Web.Endpoints;

public class Players : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetPlayer);
        groupBuilder.MapPost(CreatePlayer);
        groupBuilder.MapPut(UpdatePlayer, "{id}");
        groupBuilder.MapDelete(DeletePlayer, "{id}");
    }

    [EndpointSummary("Get a Player")]
    [EndpointDescription("Retrieves the details of a player by their ID")]
    public static async Task<Ok<PlayerVm>> GetPlayer(ISender sender)
    {
        var player = await sender.Send(new GetPlayerQuery());

        return TypedResults.Ok(player);
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
        var playerId = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Players)}/{playerId.Value}", playerId.Value);
    }

    [EndpointSummary("Update a Player")]
    [EndpointDescription(
        "Updates the specified player. The ID in the URL must match the ID in the payload."
    )]
    public static async Task<Results<NoContent, BadRequest>> UpdatePlayer(
        ISender sender,
        Guid id,
        UpdatePlayerCommand command
    )
    {
        if (id != command.Id.Value)
            return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Delete a Player")]
    [EndpointDescription(
        "Deletes the specified player. The ID in the URL must match the ID in the payload."
    )]
    public static async Task<NoContent> DeletePlayer(ISender sender, Guid id)
    {
        await sender.Send(DeletePlayerCommand.From(id));

        return TypedResults.NoContent();
    }
}
