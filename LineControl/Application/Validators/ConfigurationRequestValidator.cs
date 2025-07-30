using LineControl.Api.Contracts;
using LineControl.Domain.Interfaces;

namespace LineControl.Application.Validators;

public class ConfigurationRequestValidator : IConfigurationValidator
{
    public ValidationResult Validate(ConfigurationRequest request)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.RecipeId))
            result.Errors.Add("RecipeId is required");

        if (request.ElementsPerBag < 0)
            result.Errors.Add("ElementsPerBag must be non-negative");

        if (request.CountingSpeed is < 0 or > 100)
            result.Errors.Add("CountingSpeed must be between 0 and 100");

        if (request.FeederSpeed is < 0 or > 100)
            result.Errors.Add("FeederSpeed must be between 0 and 100");

        if (request.VibrationStrength is < 0 or > 10)
            result.Errors.Add("VibrationStrength must be between 0 and 10");

        result.IsValid = !result.Errors.Any();
        return result;
    }
}
