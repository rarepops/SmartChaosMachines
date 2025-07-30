using LineControl.Api.Contracts;
using LineControl.Application.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartChaosMachines.LineControl.Tests.Unit;

[TestClass]
public class ConfigurationValidatorTests
{
    private ConfigurationRequestValidator _validator;

    [TestInitialize]
    public void Setup()
    {
        _validator = new ConfigurationRequestValidator();
    }

    [TestMethod]
    public void Validate_ValidRequest_ReturnsValid()
    {
        // Arrange
        var request = new ConfigurationRequest
        {
            RecipeId = "RECIPE001",
            ElementsPerBag = 24,
            CountingSpeed = 50,
            FeederSpeed = 75,
            VibrationStrength = 5
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.IsTrue(result.IsValid);
        Assert.AreEqual(0, result.Errors.Count);
    }

    [TestMethod]
    public void Validate_EmptyRecipeId_ReturnsError()
    {
        // Arrange
        var request = new ConfigurationRequest
        {
            RecipeId = "",
            ElementsPerBag = 24,
            CountingSpeed = 50,
            FeederSpeed = 75,
            VibrationStrength = 5
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Contains("RecipeId is required"));
    }

    [TestMethod]
    public void Validate_NegativeElementsPerBag_ReturnsError()
    {
        // Arrange
        var request = new ConfigurationRequest
        {
            RecipeId = "RECIPE001",
            ElementsPerBag = -1,
            CountingSpeed = 50,
            FeederSpeed = 75,
            VibrationStrength = 5
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Contains("ElementsPerBag must be non-negative"));
    }

    [TestMethod]
    public void Validate_CountingSpeedOutOfRange_ReturnsError()
    {
        // Arrange
        var request = new ConfigurationRequest
        {
            RecipeId = "RECIPE001",
            ElementsPerBag = 24,
            CountingSpeed = 150,
            FeederSpeed = 75,
            VibrationStrength = 5
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Contains("CountingSpeed must be between 0 and 100"));
    }
}
