using LineControl.Api.Contracts.Responses;
using LineControl.Domain.Entities;

namespace LineControl.Application.Services;

public class MachineDataProcessor
{
    public string GetRecipeId(CountingMachineData data) => GetTag<string>(data, "RecipeId");
    public int GetCurrentCount(CountingMachineData data) => GetTag<int>(data, "CurrentCount");
    public string GetStatus(CountingMachineData data) => GetTag<string>(data, "Status");
    public int GetElementsPerBag(CountingMachineData data) => GetTag<int>(data, "ElementsPerBag");
    public int GetCountingSpeed(CountingMachineData data) => GetTag<int>(data, "CountingSpeed");
    public int GetFeederSpeed(CountingMachineData data) => GetTag<int>(data, "FeederSpeed");
    public int GetVibrationStrength(CountingMachineData data) => GetTag<int>(data, "VibrationStrength");
    public bool GetIsUsedInOrder(CountingMachineData data) => GetTag<bool>(data, "IsUsedInOrder");
    public bool HasError(CountingMachineData data) => GetTag<bool>(data, "HasError");

    public GetMachineDataResponse ProcessMachineData(CountingMachineData data)
    {
        return new GetMachineDataResponse
        {
            Position = data.Position,
            Timestamp = data.Timestamp,
            RecipeId = GetRecipeId(data),
            CurrentCount = GetCurrentCount(data),
            Status = GetStatus(data),
            ElementsPerBag = GetElementsPerBag(data),
            CountingSpeed = GetCountingSpeed(data),
            FeederSpeed = GetFeederSpeed(data),
            VibrationStrength = GetVibrationStrength(data),
            IsUsedInOrder = GetIsUsedInOrder(data),
            HasError = HasError(data)
        };
    }

    private T GetTag<T>(CountingMachineData data, string tagName)
    {
        if (data.Tags.TryGetValue(tagName, out var tagValue) && tagValue.Value is T value)
            return value;
        return default!;
    }
}
