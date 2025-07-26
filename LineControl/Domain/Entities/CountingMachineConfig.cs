namespace LineControl.Domain.Entities;

/// <summary>
/// Represents the configuration for a counting machine.
/// </summary>
public record CountingMachineConfig(
    string Position,
    Dictionary<string, TagValue> ConfigurationTags
)
{
    // Core recipe information
    public string RecipeId => GetConfigTag<string>("RecipeId");

    public int ElementsPerBag => GetConfigTag<int>("ElementsPerBag");
    public int CountingSpeed => GetConfigTag<int>("CountingSpeed");
    public int FeederSpeed => GetConfigTag<int>("BeltSpeed");
    public int VibrationStrength => GetConfigTag<int>("VibrationStrength");

    private T GetConfigTag<T>(string tagName)
    {
        if (ConfigurationTags.TryGetValue(tagName, out var tagValue) && tagValue.Value is T value)
            return value;

        return default!;
    }
}