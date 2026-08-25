using System;
using System.Collections.Generic;
using System.Text;

namespace PipAndIvory.Domain.ValueObjects;

/// <summary>
/// Represents a game variant identified by a short textual <see cref="Code"/>.
/// </summary>
/// <param name="code">The textual code for the variant (for example, "Block" or "Draw"). If <see langword="null"/> or whitespace, the <see cref="Code"/> will be set to "UNDEFINED".</param>
public class GameVariant(string code) : ValueObject
{
    /// <summary>
    /// Creates and validates a <see cref="GameVariant"/> from the provided <paramref name="code"/>.
    /// </summary>
    /// <param name="code">The code representing the desired game variant.</param>
    /// <returns>A <see cref="GameVariant"/> instance corresponding to <paramref name="code"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> does not correspond to a supported variant.</exception>
    public static GameVariant From(string code)
    {
        var variant = new GameVariant(code);

        if (!SupportedVariants.Contains(variant))
        {
            throw new ArgumentException($"The game variant '{code}' is not supported.");
        }

        return variant;
    }

    /// <summary>
    /// Represents the 'Block' game variant.
    /// </summary>
    public static GameVariant Block => new("Block");

    /// <summary>
    /// Represents the 'Draw' game variant.
    /// </summary>
    public static GameVariant Draw => new("Draw");

    /// <summary>
    /// The canonical code that identifies this game variant.
    /// </summary>
    /// <remarks>
    /// If the primary constructor receives a <see langword="null"/> or whitespace string,
    /// this property defaults to the literal "UNDEFINED".
    /// </remarks>
    public string Code { get; private set; } = string.IsNullOrWhiteSpace(code) ? "UNDEFINED" : code;

    /// <summary>
    /// Implicitly converts a <see cref="GameVariant"/> to its <see cref="Code"/> string.
    /// </summary>
    /// <param name="variant">The variant to convert.</param>
    public static implicit operator string(GameVariant variant) => variant.ToString();

    /// <summary>
    /// Explicitly converts a string code to a validated <see cref="GameVariant"/> by calling <see cref="From(string)"/>.
    /// </summary>
    /// <param name="code">The code to convert and validate.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is not a supported variant.</exception>
    public static explicit operator GameVariant(string code) => From(code);

    /// <summary>
    /// Returns the variant code.
    /// </summary>
    /// <returns>The <see cref="Code"/> string.</returns>
    public override string ToString() => Code;

    /// <summary>
    /// Gets the supported game variant instances.
    /// </summary>
    /// <remarks>
    /// This property yields the predefined, supported <see cref="GameVariant"/> instances.
    /// The sequence is implemented as an iterator and returns the canonical static instances:
    /// <list type="bullet">
    /// <item><description><see cref="Block"/> — the 'Block' variant.</description></item>
    /// <item><description><see cref="Draw"/> — the 'Draw' variant.</description></item>
    /// </list>
    /// Consumers can iterate this sequence to discover valid variants or use <see cref="From(string)"/>
    /// to validate and obtain a <see cref="GameVariant"/> instance from a string code.
    /// </remarks>
    public static IEnumerable<GameVariant> SupportedVariants
    {
        get
        {
            yield return Block;
            yield return Draw;
        }
    }

    /// <summary>
    /// Provides the components used for equality comparisons.
    /// </summary>
    /// <returns>An enumeration containing the <see cref="Code"/> component.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
