namespace PipAndIvory.Domain.ValueObjects;

/// <summary>
/// Represents a domino tile value object.
/// </summary>
/// <remarks>
/// A <see cref="Tile"/> holds two pips (spots) where a missing second pip represents a double (both pips are the same).
/// This type restricts instances to the standard double-six domino set via <see cref="From(int, int?)"/> and <see cref="SupportedTiles"/>.
/// </remarks>
public class Tile(int pip1, int? pip2 = null) : ValueObject
{
    /// <summary>
    /// Creates a <see cref="Tile"/> instance and validates it is part of the supported set.
    /// </summary>
    /// <param name="pip1">The first pip value (0-6).</param>
    /// <param name="pip2">The second pip value (0-6). If null the tile is treated as a double of <paramref name="pip1"/>.</param>
    /// <returns>A validated <see cref="Tile"/> from the allowed double-six set.</returns>
    /// <exception cref="UnsupportedTileException">Thrown when the requested tile is not part of the supported tile set.</exception>
    public static Tile From(int pip1, int? pip2 = null)
    {
        var tile = new Tile(pip1, pip2);

        if (!SupportedTiles.Contains(tile))
        {
            throw new UnsupportedTileException(tile);
        }

        return tile;
    }

    /// <summary>Tile 0-0 (double zero).</summary>
    public static Tile DoubleZero => new(0);

    /// <summary>Tile 0-1.</summary>
    public static Tile ZeroOne => new(0, 1);

    /// <summary>Tile 0-2.</summary>
    public static Tile ZeroTwo => new(0, 2);

    /// <summary>Tile 0-3.</summary>
    public static Tile ZeroThree => new(0, 3);

    /// <summary>Tile 0-4.</summary>
    public static Tile ZeroFour => new(0, 4);

    /// <summary>Tile 0-5.</summary>
    public static Tile ZeroFive => new(0, 5);

    /// <summary>Tile 0-6.</summary>
    public static Tile ZeroSix => new(0, 6);

    /// <summary>Tile 1-1 (double one).</summary>
    public static Tile DoubleOne => new(1);

    /// <summary>Tile 1-2.</summary>
    public static Tile OneTwo => new(1, 2);

    /// <summary>Tile 1-3.</summary>
    public static Tile OneThree => new(1, 3);

    /// <summary>Tile 1-4.</summary>
    public static Tile OneFour => new(1, 4);

    /// <summary>Tile 1-5.</summary>
    public static Tile OneFive => new(1, 5);

    /// <summary>Tile 1-6.</summary>
    public static Tile OneSix => new(1, 6);

    /// <summary>Tile 2-2 (double two).</summary>
    public static Tile DoubleTwo => new(2);

    /// <summary>Tile 2-3.</summary>
    public static Tile TwoThree => new(2, 3);

    /// <summary>Tile 2-4.</summary>
    public static Tile TwoFour => new(2, 4);

    /// <summary>Tile 2-5.</summary>
    public static Tile TwoFive => new(2, 5);

    /// <summary>Tile 2-6.</summary>
    public static Tile TwoSix => new(2, 6);

    /// <summary>Tile 3-3 (double three).</summary>
    public static Tile DoubleThree => new(3);

    /// <summary>Tile 3-4.</summary>
    public static Tile ThreeFour => new(3, 4);

    /// <summary>Tile 3-5.</summary>
    public static Tile ThreeFive => new(3, 5);

    /// <summary>Tile 3-6.</summary>
    public static Tile ThreeSix => new(3, 6);

    /// <summary>Tile 4-4 (double four).</summary>
    public static Tile DoubleFour => new(4);

    /// <summary>Tile 4-5.</summary>
    public static Tile FourFive => new(4, 5);

    /// <summary>Tile 4-6.</summary>
    public static Tile FourSix => new(4, 6);

    /// <summary>Tile 5-5 (double five).</summary>
    public static Tile DoubleFive => new(5);

    /// <summary>Tile 5-6.</summary>
    public static Tile FiveSix => new(5, 6);

    /// <summary>Tile 6-6 (double six).</summary>
    public static Tile DoubleSix => new(6);

    /// <summary>
    /// First pip value of the tile.
    /// </summary>
    public int Pip1 { get; private set; } = pip1;

    /// <summary>
    /// Second pip value of the tile. For doubles this equals <see cref="Pip1"/>.
    /// </summary>
    public int Pip2 { get; private set; } = pip2 ?? pip1;

    /// <summary>
    /// Human-readable name in the form "pip1-pip2" (for example "3-6" or "4-4").
    /// </summary>
    public string Name => $"{Pip1}-{Pip2}";

    /// <summary>
    /// Implicit conversion to <see cref="string"/> producing the same value as <see cref="ToString"/>.
    /// </summary>
    /// <param name="tile">The tile to convert.</param>
    public static implicit operator string(Tile tile)
    {
        return tile.ToString();
    }

    /// <summary>
    /// Explicit conversion from a string in the form "pip1-pip2" to a validated <see cref="Tile"/>.
    /// </summary>
    /// <param name="name">The tile name to parse (e.g. "2-5" or "4").</param>
    /// <returns>A validated <see cref="Tile"/> instance.</returns>
    /// <exception cref="UnsupportedTileException">Thrown when the parsed tile is not part of the supported set.</exception>
    public static explicit operator Tile(string name)
    {
        var parts = name.Split('-');
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
    /// Enumerates all tiles in the supported standard double-six set.
    /// </summary>
    public static IEnumerable<Tile> SupportedTiles
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
}
