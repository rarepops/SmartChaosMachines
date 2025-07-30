using LineControl.Api.Contracts;

namespace LineControl.Domain.Interfaces;

public interface IConfigurationValidator
{
    ValidationResult Validate(ConfigurationRequest request);
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}
