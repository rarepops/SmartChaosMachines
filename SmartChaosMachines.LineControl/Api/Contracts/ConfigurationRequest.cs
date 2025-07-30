namespace LineControl.Api.Contracts;

public class ConfigurationRequest
{
    public required string RecipeId { get; set; }
    public required int ElementsPerBag { get; set; }
    public required int CountingSpeed { get; set; }
    public required int FeederSpeed { get; set; }
    public required int VibrationStrength { get; set; }
}
