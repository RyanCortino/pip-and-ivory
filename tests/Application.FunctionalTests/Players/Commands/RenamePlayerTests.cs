using PipAndIvory.Application.Common.Exceptions;
using PipAndIvory.Application.Players.Commands.RegisterPlayer;
using PipAndIvory.Application.Players.Commands.RenamePlayer;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.FunctionalTests.Players.Commands;

public class RenamePlayerTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new RenamePlayerCommand
        {
            PlayerId = PlayerId.New,
            DisplayName = "New Player",
        };

        await Should.ThrowAsync<NotFoundException>(() => TestApp.SendAsync(command));
    }

    [Test]
    public async Task ShouldRenamePlayer()
    {
        var userId = await TestApp.RunAsDefaultUserAsync();

        var playerId = await TestApp.SendAsync(
            new RegisterPlayerCommand { DisplayName = "New Player" }
        );

        var command = new RenamePlayerCommand
        {
            PlayerId = playerId,
            DisplayName = "Renamed Player",
        };

        await TestApp.SendAsync(command);

        var player = await TestApp.FindAsync<Player>(playerId);
    }
}
