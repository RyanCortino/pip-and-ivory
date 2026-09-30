namespace PipAndIvory.Domain.ValueObjects.ReferenceTypes;

public class PlayerId(Guid value) : ReferenceTypeId<PlayerId>(value)
{
    private static readonly string[] Adjectives =
    {
        "Sneaky",
        "Swift",
        "Mighty",
        "Frosty",
        "Golden",
        "Shadow",
        "Cosmic",
        "Cranky",
        "Brave",
        "Lively",
    };

    private static readonly string[] Nouns =
    {
        "Panda",
        "Raptor",
        "Knight",
        "Wizard",
        "Badger",
        "Falcon",
        "Coyote",
        "Ninja",
        "Goblin",
        "Phoenix",
    };

    public static string GenerateFriendlyName(PlayerId playerId)
    {
        // Extract the raw 16 bytes from the GUID
        byte[] bytes = playerId.Value.ToByteArray();

        // Convert chunks of bytes into integers using BitConverter
        // BitConverter.ToInt32 requires 4 bytes
        int adjIndex = Math.Abs(BitConverter.ToInt32(bytes, 0));
        int nounIndex = Math.Abs(BitConverter.ToInt32(bytes, 4));
        int suffixNumber = Math.Abs(BitConverter.ToInt32(bytes, 8));

        // Use modulo (%) to map the large integers to your list sizes
        string selectedAdj = Adjectives[adjIndex % Adjectives.Length];
        string selectedNoun = Nouns[nounIndex % Nouns.Length];

        // Create a 4 - digit numeric suffix(0000 to 9999) for extra safety
        int finalSuffix = suffixNumber % 10000;

        // Combine them into a display-friendly name
        return $"{selectedAdj}{selectedNoun}#{finalSuffix:D4}";
    }
}
