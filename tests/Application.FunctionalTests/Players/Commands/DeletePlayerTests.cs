using PipAndIvory.Application.Players.Commands.CreatePlayer;
using PipAndIvory.Application.Players.Commands.DeletePlayer;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.FunctionalTests.Players.Commands;

public class DeletePlayerTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidPlayerId()
    {
        var command = new DeletePlayerCommand(default(PlayerId));
        await Should.ThrowAsync<NotFoundException>(() => TestApp.SendAsync(command));
    }

    [Test]
    public async Task ShouldDeletePlayer()
    {
        var playerId = await TestApp.SendAsync(
            new CreatePlayerCommand { DisplayName = "Test Player" }
        );

        await TestApp.SendAsync(new DeletePlayerCommand(playerId));

        var player = await TestApp.FindAsync<Player>(playerId);

        player.ShouldBeNull();
    }
}
