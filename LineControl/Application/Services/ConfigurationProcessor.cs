using LineControl.Domain.Entities;

namespace LineControl.Application.Services;

public class ConfigurationProcessor
{
    public string GetRecipeId(CountingMachineConfig config) => GetConfigTag<string>(config, "RecipeId");
    public int GetElementsPerBag(CountingMachineConfig config) => GetConfigTag<int>(config, "ElementsPerBag");
    public int GetCountingSpeed(CountingMachineConfig config) => GetConfigTag<int>(config, "CountingSpeed");
    public int GetFeederSpeed(CountingMachineConfig config) => GetConfigTag<int>(config, "FeederSpeed");
    public int GetVibrationStrength(CountingMachineConfig config) => GetConfigTag<int>(config, "VibrationStrength");

    private T GetConfigTag<T>(CountingMachineConfig config, string tagName)
    {
        if (config.ConfigurationTags.TryGetValue(tagName, out var tagValue) && tagValue.Value is T value)
            return value;
        return default!;
    }
}
