using PipAndIvory.Application.Games.Commands.StartGame;
using PipAndIvory.Application.Players.Commands.RegisterPlayer;
using PipAndIvory.Domain.Entities;
using PipAndIvory.Domain.Enums;
using PipAndIvory.Domain.ValueObjects;
using PipAndIvory.Domain.ValueObjects.ReferenceTypes;

namespace PipAndIvory.Application.FunctionalTests.Games.Commands;

public class StartGameTests : TestBase
{
    [Test]
    public async Task ShouldCreateDefaultGame_HasDefaultVariantAndTwoParticipants()
    {
        var userId = await TestApp.RunAsDefaultUserAsync();

        var playerOneId = await TestApp.SendAsync(
            new RegisterPlayerCommand { DisplayName = "Player 1" }
        );

        var playerTwoId = await TestApp.SendAsync(
            new RegisterPlayerCommand { DisplayName = "Player 2" }
        );

        var command = new StartGameCommand { PlayerIds = [playerOneId, playerTwoId] };

        var gameId = await TestApp.SendAsync(command);

        var game = await TestApp.FindAsync<Game>(gameId);

        game.ShouldNotBeNull();
        game!.GameVariant.ShouldBe(GameVariant.Block);
        game.Participants.ShouldNotBeNull();
        game.Participants.Count.ShouldBe(2);
        game.CreatedBy.ShouldBe(userId);
        game.Created.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        game.LastModifiedBy.ShouldBe(userId);
        game.LastModified.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
