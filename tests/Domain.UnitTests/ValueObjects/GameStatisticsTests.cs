using NUnit.Framework;
using PipAndIvory.Domain.Exceptions;
using PipAndIvory.Domain.ValueObjects;
using Shouldly;

namespace PipAndIvory.Domain.UnitTests.ValueObjects;

public class GameStatisticsTests
{
    [Test]
    public void ShouldCreateInstanceGivenValidValues()
    {
        var gs = GameStatistics.From(10, 3, 50);

        gs.Played.ShouldBe(10);
        gs.Won.ShouldBe(3);
        gs.HighestScore.ShouldBe(50);
    }

    [TestCase(-1, 0, 0)]
    [TestCase(0, -1, 0)]
    [TestCase(0, 0, -1)]
    public void ShouldThrowUnsupportedGameStatisticsExceptionGivenNegativeValues(
        int played,
        int won,
        int highest
    )
    {
        Should.Throw<UnsupportedGameStatisticsException>(
            () => GameStatistics.From(played, won, highest)
        );
    }

    [Test]
    public void ShouldThrowUnsupportedGameStatisticsExceptionWhenWonGreaterThanPlayed()
    {
        Should.Throw<UnsupportedGameStatisticsException>(() => GameStatistics.From(1, 2, 0));
    }

    [Test]
    public void ShouldReturnNoneWithZeroedValues()
    {
        var none = GameStatistics.None;

        none.Played.ShouldBe(0);
        none.Won.ShouldBe(0);
        none.HighestScore.ShouldBe(0);
    }

    [Test]
    public void ShouldRecordGameAsWin_IncrementsPlayedAndWonAndUpdatesHighestScore()
    {
        var gs = GameStatistics.From(2, 1, 10);

        var recorded = gs.RecordGame(true, 12);

        recorded.Played.ShouldBe(3);
        recorded.Won.ShouldBe(2);
        recorded.HighestScore.ShouldBe(12);
    }

    [Test]
    public void ShouldRecordGameAsLoss_IncrementsPlayedButNotWonAndPreservesHigherScore()
    {
        var gs = GameStatistics.From(2, 1, 10);

        var recorded = gs.RecordGame(false, 5);

        recorded.Played.ShouldBe(3);
        recorded.Won.ShouldBe(1);
        recorded.HighestScore.ShouldBe(10);
    }

    [Test]
    public void ShouldBeComparableWithOperators()
    {
        var a = GameStatistics.From(4, 2, 20);
        var b = GameStatistics.From(4, 2, 20);
        var c = GameStatistics.From(5, 2, 20);

        (a == b).ShouldBe(true);
        (a == c).ShouldBe(false);

        a.Equals(b).ShouldBeTrue();
        a.Equals(c).ShouldBeFalse();
    }

    [Test]
    public void ShouldHaveSameHashCodeForEqualInstances()
    {
        var a = GameStatistics.From(4, 2, 20);
        var b = GameStatistics.From(4, 2, 20);

        a.GetHashCode().ShouldBe(b.GetHashCode());
    }
}
