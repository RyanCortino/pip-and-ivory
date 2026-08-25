namespace PipAndIvory.Domain.ValueObjects;

/// <summary>
/// Represents a cardinal or intercardinal direction as a value object.
/// The value is stored as an uppercase direction code (for example: "N", "NE", "E").
/// </summary>
/// <remarks>
/// Use the static properties (e.g. <see cref="North"/>, <see cref="SouthWest"/>) to obtain common directions,
/// or <see cref="From(string)"/> / explicit conversion to construct a validated instance from a string.
/// </remarks>
public class CardinalDirection(string code) : ValueObject
{
    /// <summary>
    /// Creates a <see cref="CardinalDirection"/> from the provided <paramref name="code"/>,
    /// validating that the code is one of the supported directions.
    /// </summary>
    /// <param name="code">The direction code to construct (case-insensitive). Valid values: "N","NE","E","SE","S","SW","W","NW".</param>
    /// <returns>A validated <see cref="CardinalDirection"/> instance.</returns>
    /// <exception cref="UnsupportedCardinalDirectionException">Thrown when <paramref name="code"/> is not a supported direction.</exception>
    public static CardinalDirection From(string code)
    {
        var direction = new CardinalDirection(code);

        if (!SupportedDirections.Contains(direction))
        {
            throw new UnsupportedCardinalDirectionException(code);
        }
        return direction;
    }

    /// <summary>Represents North ("N").</summary>
    public static CardinalDirection North => new("N");

    /// <summary>Represents North-East ("NE").</summary>
    public static CardinalDirection NorthEast => new("NE");

    /// <summary>Represents East ("E").</summary>
    public static CardinalDirection East => new("E");

    /// <summary>Represents South-East ("SE").</summary>
    public static CardinalDirection SouthEast => new("SE");

    /// <summary>Represents South ("S").</summary>
    public static CardinalDirection South => new("S");

    /// <summary>Represents South-West ("SW").</summary>
    public static CardinalDirection SouthWest => new("SW");

    /// <summary>Represents West ("W").</summary>
    public static CardinalDirection West => new("W");

    /// <summary>Represents North-West ("NW").</summary>
    public static CardinalDirection NorthWest => new("NW");

    /// <summary>
    /// Implicitly converts a <see cref="CardinalDirection"/> to its string representation.
    /// </summary>
    /// <param name="direction">The direction to convert.</param>
    /// <returns>The direction code string (e.g. "N", "NE").</returns>
    public static implicit operator string(CardinalDirection direction)
    {
        return direction.ToString();
    }

    /// <summary>
    /// Explicitly converts a string into a validated <see cref="CardinalDirection"/> instance.
    /// This performs the same validation as <see cref="From(string)"/>.
    /// </summary>
    /// <param name="code">The direction code to convert.</param>
    /// <returns>A validated <see cref="CardinalDirection"/>.</returns>
    /// <exception cref="UnsupportedCardinalDirectionException">When <paramref name="code"/> is not supported.</exception>
    public static explicit operator CardinalDirection(string code)
    {
        return From(code);
    }

    /// <summary>
    /// Returns the normalized direction code.
    /// </summary>
    /// <returns>The direction code string.</returns>
    public override string ToString()
    {
        return Code;
    }

    /// <summary>
    /// The normalized direction code stored by this instance.
    /// If the provided constructor <paramref name="code"/> is null/whitespace, this will be "UNDEFINED".
    /// </summary>
    public string Code { get; private set; } =
        string.IsNullOrWhiteSpace(code) ? "UNDEFINED" : code.ToUpper();

    /// <summary>
    /// Enumerates all supported canonical directions (N, NE, E, SE, S, SW, W, NW).
    /// Useful for validation and iteration.
    /// </summary>
    public static IEnumerable<CardinalDirection> SupportedDirections
    {
        get
        {
            yield return North;
            yield return NorthEast;
            yield return East;
            yield return SouthEast;
            yield return South;
            yield return SouthWest;
            yield return West;
            yield return NorthWest;
        }
    }

    /// <summary>
    /// Supplies the equality components used by the base <see cref="ValueObject"/> implementation.
    /// </summary>
    /// <returns>The sequence of components that determine equality (the <see cref="Code"/>).</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
