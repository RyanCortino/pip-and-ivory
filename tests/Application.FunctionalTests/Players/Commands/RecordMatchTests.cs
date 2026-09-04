using PipAndIvory.Application.Players.Commands.CreatePlayer;
using PipAndIvory.Application.Players.Commands.RecordMatch;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.FunctionalTests.Players.Commands;

public class RecordMatchTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidPlayerId()
    {
        var command = new RecordMatchCommand
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
            new CreatePlayerCommand { DisplayName = "Test Player" }
        );

        var command = new RecordMatchCommand
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
