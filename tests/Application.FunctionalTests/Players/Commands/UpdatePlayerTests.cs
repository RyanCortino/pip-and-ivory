using PipAndIvory.Application.Common.Exceptions;
using PipAndIvory.Application.Players.Commands.CreatePlayer;
using PipAndIvory.Application.Players.Commands.UpdatePlayer;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.FunctionalTests.Players.Commands;

public class UpdatePlayerTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidPlayerId()
    {
        var command = new UpdatePlayerCommand { Id = PlayerId.New(), DisplayName = "Name" };
        await Should.ThrowAsync<NotFoundException>(() => TestApp.SendAsync(command));
    }

    [Test]
    public async Task ShouldRequireDisplayName()
    {
        var playerId = await TestApp.SendAsync(
            new CreatePlayerCommand { DisplayName = "Initial Player" }
        );

        var command = new UpdatePlayerCommand { Id = playerId, DisplayName = string.Empty };

        var ex = await Should.ThrowAsync<ValidationException>(() => TestApp.SendAsync(command));

        ex.Errors.ShouldContainKey("DisplayName");
        ex.Errors["DisplayName"].ShouldContain("'Display Name' must not be empty.");
    }

    [Test]
    public async Task ShouldUpdatePlayer()
    {
        var userId = await TestApp.RunAsDefaultUserAsync();

        var playerId = await TestApp.SendAsync(
            new CreatePlayerCommand { DisplayName = "Initial Player" }
        );

        var command = new UpdatePlayerCommand { Id = playerId, DisplayName = "Updated Player" };

        await TestApp.SendAsync(command);

        var player = await TestApp.FindAsync<Player>(playerId);

        player.ShouldNotBeNull();
        player!.DisplayName.ShouldBe(command.DisplayName);
        player.LastModifiedBy.ShouldNotBeNull();
        player.LastModifiedBy.ShouldBe(userId);
        player.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
