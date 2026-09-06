using PipAndIvory.Application.Players.Commands.RecordGameResult;
using PipAndIvory.Application.Players.Commands.RegisterPlayer;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.FunctionalTests.Players.Commands;

public class RecordGameResultTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidPlayerId()
    {
        var command = new RecordGameResultCommand
        {
            PlayerId = PlayerId.New(),
            Won = true,
            Score = 10,
        };

        await Should.ThrowAsync<NotFoundException>(() => TestApp.SendAsync(command));
    }

    [Test]
    public async Task ShouldUpdatePlayer()
    {
        var userId = await TestApp.RunAsDefaultUserAsync();

        var playerId = await TestApp.SendAsync(
            new RegisterPlayerCommand { DisplayName = "Test Player" }
        );

        var command = new RecordGameResultCommand
        {
            PlayerId = playerId,
            Gamemode = GameVariant.Block,
            Won = true,
            Score = 10,
        };

        await TestApp.SendAsync(command);

        var player = await TestApp.FindAsync<Player>(playerId);

        player.ShouldNotBeNull();
        player!.BlockGameStats.Played.ShouldBe(1);
        player.BlockGameStats.Won.ShouldBe(1);
        player.BlockGameStats.HighestScore.ShouldBe(10);
        player.LastModifiedBy.ShouldNotBeNull();
        player.LastModifiedBy.ShouldBe(userId);
        player.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
