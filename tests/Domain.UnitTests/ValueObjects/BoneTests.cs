using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PipAndIvory.Domain.Exceptions;
using PipAndIvory.Domain.ValueObjects;
using Shouldly;

namespace PipAndIvory.Domain.UnitTests.ValueObjects;

public class BoneTests
{
    [Test]
    public void ShouldReturnCorrectPipValues()
    {
        var bone = Bone.From(3, 6);

        bone.Pip1.ShouldBe(3);
        bone.Pip2.ShouldBe(6);
        bone.ShouldBe(Bone.ThreeSix);
    }

    [Test]
    public void ShouldReturnCorrectPipValuesForDoubles()
    {
        var bone = Bone.From(4, 4);

        bone.Pip1.ShouldBe(4);
        bone.Pip2.ShouldBe(4);
        bone.ShouldBe(Bone.DoubleFour);
    }

    [Test]
    public void ToStringReturnsName()
    {
        var bone = Bone.ThreeSix;

        bone.ToString().ShouldBe(bone.Name);
    }

    [Test]
    public void ShouldPerformImplicitConversionToBoneNameString()
    {
        string name = Bone.ThreeSix;

        name.ShouldBe("[3|6]");
    }

    [Test]
    public void ShouldPerformExplicitConversionGivenSupportedBoneName()
    {
        var bone = (Bone)"[4|4]";

        bone.ShouldBe(Bone.DoubleFour);
    }

    [Test]
    public void ShouldPerformHasFaceAndWeightCalculations()
    {
        var bone = Bone.From(2, 5);

        Assert.True(bone.HasFace(2));
        Assert.True(bone.HasFace(5));
        Assert.False(bone.HasFace(3));
        Assert.AreEqual(7, bone.Weight);
    }

    [Test]
    public void ShouldBeComparableWithOperators()
    {
        var b1 = Bone.From(3, 6);
        var b2 = Bone.From(3, 6);
        var b3 = Bone.From(6, 6);

        (b1 == b2).ShouldBe(true);
        (b1 == b3).ShouldBe(false);
    }

    [Test]
    public void ShouldRecognizeCannonicalOrientation()
    {
        var b1 = Bone.From(3, 5);
        var b2 = Bone.From(5, 3);

        (b1 == b2).ShouldBe(true);
    }

    [Test]
    public void ShouldThrowUnsupportedBoneExceptionGivenOutOfRangePipValue()
    {
        Should.Throw<UnsupportedBoneException>(() => Bone.From(-1, 3));
        Should.Throw<UnsupportedBoneException>(() => Bone.From(0, 8));
    }

    [Test]
    public void WeightIsSumOfPips_ForNonDoubleBone()
    {
        var bone = Bone.From(2, 5);

        bone.Weight.ShouldBe(7); // 2 + 5 = 7
    }

    [Test]
    public void WeightIsZeroForZeroZero()
    {
        var bone = Bone.DoubleZero;

        bone.Weight.ShouldBe(0); // 0 + 0 = 0
    }
}
