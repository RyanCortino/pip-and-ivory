using PipAndIvory.Application.Players.Commands.RegisterPlayer;
using PipAndIvory.Domain.Entities;

namespace PipAndIvory.Application.FunctionalTests.Players.Commands;

public class RegisterPlayerTests : TestBase
{
    [Test]
    public async Task ShouldCreatePlayer()
    {
        var userId = await TestApp.RunAsDefaultUserAsync();

        var command = new RegisterPlayerCommand { DisplayName = "Alice" };

        var playerId = await TestApp.SendAsync(command);

        var player = await TestApp.FindAsync<Player>(playerId);

        player.ShouldNotBeNull();
        player!.DisplayName.ShouldBe(command.DisplayName);
        player.CreatedBy.ShouldBe(userId);
        player.Created.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        player.LastModifiedBy.ShouldBe(userId);
        player.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }

    [Test]
    public async Task ShouldCreatePlayer_WhenDisplayNameIsMissing()
    {
        var userId = await TestApp.RunAsDefaultUserAsync();

        var command = new RegisterPlayerCommand();

        var playerId = await TestApp.SendAsync(command);

        var player = await TestApp.FindAsync<Player>(playerId);

        player.ShouldNotBeNull();
        player!.DisplayName.ShouldBeNull();
        player.CreatedBy.ShouldBe(userId);
        player.Created.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        player.LastModifiedBy.ShouldBe(userId);
        player.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
