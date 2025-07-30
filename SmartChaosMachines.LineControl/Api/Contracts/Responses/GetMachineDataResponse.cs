namespace LineControl.Api.Contracts.Responses;

public class GetMachineDataResponse
{
    public required string Position { get; set; }
    public DateTime Timestamp { get; set; }
    public required string RecipeId { get; set; }
    public int CurrentCount { get; set; }
    public required string Status { get; set; }
    public int ElementsPerBag { get; set; }
    public int CountingSpeed { get; set; }
    public int FeederSpeed { get; set; }
    public int VibrationStrength { get; set; }
    public bool IsUsedInOrder { get; set; }
    public bool HasError { get; set; }
}
