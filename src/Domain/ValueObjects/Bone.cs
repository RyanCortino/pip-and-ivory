using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PipAndIvory.Domain.ValueObjects;

/// <summary>
/// Represents a domino value object.
/// </summary>
/// <remarks>
/// A <see cref="Bone"/> holds two pips (spots) where a missing second pip represents a double (both pips are the same).
/// This type restricts instances to the standard double-six domino set via <see cref="From(int, int?)"/> and <see cref="SupportedBones"/>.
/// </remarks>
public partial class Bone(int pip1, int pip2) : ValueObject
{
    /// <summary>
    /// Creates a <see cref="Bone"/> instance and validates it is part of the supported set.
    /// </summary>
    /// <param name="pip1">The first pip value (0-6).</param>
    /// <param name="pip2">The second pip value (0-6). If null the bone is treated as a double of <paramref name="pip1"/>.</param>
    /// <returns>A validated <see cref="Bone"/> from the allowed double-six set.</returns>
    /// <exception cref="UnsupportedBoneException">Thrown when the requested bone is not part of the supported bone set.</exception>
    public static Bone From(int pip1, int? pip2 = null)
    {
        int secondFace = pip2 ?? pip1;
        int firstFace = pip1;

        Bone bone =
            firstFace <= secondFace
                ? new Bone(firstFace, secondFace)
                : new Bone(secondFace, firstFace);

        if (IsOutOfRange(bone.Pip1) || IsOutOfRange(bone.Pip2))
        {
            throw new UnsupportedBoneException(bone);
        }

        if (!SupportedBones.Contains(bone))
        {
            throw new UnsupportedBoneException(bone);
        }

        return bone;

        static bool IsOutOfRange(int pip)
        {
            return pip is < 0 or > 6;
        }
    }

    /// <summary>Bone 0-0 (double zero).</summary>
    public static Bone DoubleZero => new(0, 0);

    /// <summary>Bone 0-1.</summary>
    public static Bone ZeroOne => new(0, 1);

    /// <summary>Bone 0-2.</summary>
    public static Bone ZeroTwo => new(0, 2);

    /// <summary>Bone 0-3.</summary>
    public static Bone ZeroThree => new(0, 3);

    /// <summary>Bone 0-4.</summary>
    public static Bone ZeroFour => new(0, 4);

    /// <summary>Bone 0-5.</summary>
    public static Bone ZeroFive => new(0, 5);

    /// <summary>Bone 0-6.</summary>
    public static Bone ZeroSix => new(0, 6);

    /// <summary>Bone 1-1 (double one).</summary>
    public static Bone DoubleOne => new(1, 1);

    /// <summary>Bone 1-2.</summary>
    public static Bone OneTwo => new(1, 2);

    /// <summary>Bone 1-3.</summary>
    public static Bone OneThree => new(1, 3);

    /// <summary>Bone 1-4.</summary>
    public static Bone OneFour => new(1, 4);

    /// <summary>Bone 1-5.</summary>
    public static Bone OneFive => new(1, 5);

    /// <summary>Bone 1-6.</summary>
    public static Bone OneSix => new(1, 6);

    /// <summary>Bone 2-2 (double two).</summary>
    public static Bone DoubleTwo => new(2, 2);

    /// <summary>Bone 2-3.</summary>
    public static Bone TwoThree => new(2, 3);

    /// <summary>Bone 2-4.</summary>
    public static Bone TwoFour => new(2, 4);

    /// <summary>Bone 2-5.</summary>
    public static Bone TwoFive => new(2, 5);

    /// <summary>Bone 2-6.</summary>
    public static Bone TwoSix => new(2, 6);

    /// <summary>Bone 3-3 (double three).</summary>
    public static Bone DoubleThree => new(3, 3);

    /// <summary>Bone 3-4.</summary>
    public static Bone ThreeFour => new(3, 4);

    /// <summary>Bone 3-5.</summary>
    public static Bone ThreeFive => new(3, 5);

    /// <summary>Bone 3-6.</summary>
    public static Bone ThreeSix => new(3, 6);

    /// <summary>Bone 4-4 (double four).</summary>
    public static Bone DoubleFour => new(4, 4);

    /// <summary>Bone 4-5.</summary>
    public static Bone FourFive => new(4, 5);

    /// <summary>Bone 4-6.</summary>
    public static Bone FourSix => new(4, 6);

    /// <summary>Bone 5-5 (double five).</summary>
    public static Bone DoubleFive => new(5, 5);

    /// <summary>Bone 5-6.</summary>
    public static Bone FiveSix => new(5, 6);

    /// <summary>Bone 6-6 (double six).</summary>
    public static Bone DoubleSix => new(6, 6);

    /// <summary>
    /// First pip value of the bone.
    /// </summary>
    public int Pip1 { get; private set; } = pip1;

    /// <summary>
    /// Second pip value of the bone. For doubles this equals <see cref="Pip1"/>.
    /// </summary>
    public int Pip2 { get; private set; } = pip2;

    /// <summary>
    /// Human-readable name in the form "pip1-pip2" (for example "3-6" or "4-4").
    /// </summary>
    public string Name => $"[{Pip1}|{Pip2}]";

    /// <summary>
    /// Total pip count of the bone, calculated as the sum of <see cref="Pip1"/> and <see cref="Pip2"/>.
    /// </summary>
    public int Weight => Pip1 + Pip2;

    /// <summary>
    /// Indicates whether the bone is a double (both pips are the same).
    /// </summary>
    public bool IsDouble => Pip1 == Pip2;

    /// <summary>
    /// Indicates whether the bone has a pip matching the specified value.
    /// </summary>
    /// <param name="value">The pip value to check.</param>
    /// <returns>True if the bone has a pip matching the specified value; otherwise, false.</returns>
    public bool HasFace(int value) => Pip1 == value || Pip2 == value;

    /// <summary>
    /// Implicit conversion to <see cref="string"/> producing the same value as <see cref="ToString"/>.
    /// </summary>
    /// <param name="bone">The bone to convert.</param>
    public static implicit operator string(Bone bone)
    {
        return bone.ToString();
    }

    /// <summary>
    /// Explicit conversion from a string in the form "pip1-pip2" to a validated <see cref="Bone"/>.
    /// </summary>
    /// <param name="name">The bone name to parse (e.g. "2-5" or "4").</param>
    /// <returns>A validated <see cref="Bone"/> instance.</returns>
    /// <exception cref="UnsupportedBoneException">Thrown when the parsed bone is not part of the supported set.</exception>
    public static explicit operator Bone(string name)
    {
        var parts = name.Split('|')
            .Select(p => NonNumerical().Replace(p ?? string.Empty, string.Empty))
            .ToArray();

        int pip1 = int.Parse(parts[0]);
        int? pip2 = parts.Length > 1 ? int.Parse(parts[1]) : null;

        return From(pip1, pip2);
    }

    /// <summary>
    /// Returns the <see cref="Name"/> representation.
    /// </summary>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Enumerates all bones in the supported standard double-six set.
    /// </summary>
    public static IEnumerable<Bone> SupportedBones
    {
        get
        {
            // Complete Double-Six set of dominoes
            yield return DoubleZero;
            yield return ZeroOne;
            yield return ZeroTwo;
            yield return ZeroThree;
            yield return ZeroFour;
            yield return ZeroFive;
            yield return ZeroSix;
            yield return DoubleOne;
            yield return OneTwo;
            yield return OneThree;
            yield return OneFour;
            yield return OneFive;
            yield return OneSix;
            yield return DoubleTwo;
            yield return TwoThree;
            yield return TwoFour;
            yield return TwoFive;
            yield return TwoSix;
            yield return DoubleThree;
            yield return ThreeFour;
            yield return ThreeFive;
            yield return ThreeSix;
            yield return DoubleFour;
            yield return FourFive;
            yield return FourSix;
            yield return DoubleFive;
            yield return FiveSix;
            yield return DoubleSix;
        }
    }

    /// <summary>
    /// Provides the components used for equality comparison: <see cref="Pip1"/> and <see cref="Pip2"/>.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Pip1;
        yield return Pip2;
    }

    [GeneratedRegex("[^0-9]")]
    private static partial Regex NonNumerical();
}
