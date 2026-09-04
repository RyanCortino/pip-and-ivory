using Microsoft.AspNetCore.Mvc.Infrastructure;
using PipAndIvory.Application.Players.Queries.GetPlayer;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.FunctionalTests.Players.Queries;

public class GetPlayerTests : TestBase
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetPlayerQuery();

        var action = () => TestApp.SendAsync(query);

        await Should.ThrowAsync<UnauthorizedAccessException>(action);
    }

    [Test]
    public async Task ShouldReturnPlayerVmGivenPlayerExists()
    {
        var userId = await TestApp.RunAsDefaultUserAsync();

        var TestDisplayName = "Test Player";

        var player = new Player { Id = PlayerId.New(), DisplayName = TestDisplayName };

        player.RecordGameResult(GameVariant.Block, true, 100);

        await TestApp.AddAsync(player);

        var result = await TestApp.SendAsync(new GetPlayerQuery());

        result.ShouldNotBeNull();
        result.Player.ShouldNotBeNull();
        result.Player.DisplayName.ShouldBe(TestDisplayName);
        result.Player.Id.ShouldBe(player.Id.ToString());
        result.Player.BlockGameStats.ShouldNotBeNull();
        result.Player.BlockGameStats.Played.ShouldBe(1);
        result.Player.BlockGameStats.Won.ShouldBe(1);
        result.Player.BlockGameStats.HighestScore.ShouldBe(100);
        result.Player.DrawGameStats.ShouldNotBeNull();
        result.Player.DrawGameStats.Played.ShouldBe(0);
        result.Player.DrawGameStats.Won.ShouldBe(0);
        result.Player.DrawGameStats.HighestScore.ShouldBe(0);
    }
}
