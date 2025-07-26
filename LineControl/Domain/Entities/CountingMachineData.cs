namespace LineControl.Domain.Entities;

/// <summary>
/// Represents a snapshot of data from a counting machine.
/// </summary>
public record CountingMachineData(
    string Position,
    DateTime Timestamp,
    Dictionary<string, TagValue> Tags
)
{
    public string RecipeId => GetTag<string>("RecipeId");
    public int CurrentCount => GetTag<int>("CurrentCount");
    public string Status => GetTag<string>("Status");

    public int ElementsPerBag => GetTag<int>("ElementsPerBag");
    public int CountingSpeed => GetTag<int>("CountingSpeed");
    public int FeederSpeed => GetTag<int>("BeltSpeed");
    public int VibrationStrength => GetTag<int>("VibrationStrength");

    public bool IsUsedInOrder => GetTag<bool>("IsUsedInOrder");
    public bool HasError => GetTag<bool>("HasError");

    private T GetTag<T>(string tagName)
    {
        if (Tags.TryGetValue(tagName, out var tagValue) && tagValue.Value is T value)
            return value;

        return default!;
    }
}