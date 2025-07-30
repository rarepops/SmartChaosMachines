using LineControl.Api.Contracts;
using LineControl.Application.UseCases.ConfigureMachine;
using LineControl.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace SmartChaosMachines.LineControl.Tests.Unit;

[TestClass]
public class ConfigureMachineUseCaseTests
{
    private Mock<IMachineRepository> _mockRepository;
    private Mock<IConfigurationValidator> _mockValidator;
    private ConfigureMachineUseCase _useCase;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IMachineRepository>();
        _mockValidator = new Mock<IConfigurationValidator>();
        _useCase = new ConfigureMachineUseCase(_mockRepository.Object, _mockValidator.Object);
    }

    [TestMethod]
    public async Task ExecuteAsync_ValidConfiguration_ReturnsTrue()
    {
        // Arrange
        var request = new ConfigurationRequest
        {
            RecipeId = "TEST001",
            ElementsPerBag = 24,
            CountingSpeed = 50,
            FeederSpeed = 75,
            VibrationStrength = 3
        };

        _mockValidator.Setup(v => v.Validate(request))
            .Returns(new ValidationResult { IsValid = true });
        _mockRepository.Setup(r => r.ApplyConfigurationAsync("CM01_1", request))
            .ReturnsAsync(true);

        // Act
        var result = await _useCase.ExecuteAsync("CM01_1", request);

        // Assert
        Assert.IsTrue(result);
        _mockValidator.Verify(v => v.Validate(request), Times.Once);
        _mockRepository.Verify(r => r.ApplyConfigurationAsync("CM01_1", request), Times.Once);
    }

    [TestMethod]
    public async Task ExecuteAsync_InvalidConfiguration_ReturnsFalse()
    {
        // Arrange
        var request = new ConfigurationRequest
        {
            RecipeId = "",
            ElementsPerBag = -1,
            CountingSpeed = 150,
            FeederSpeed = 75,
            VibrationStrength = 3
        };

        _mockValidator.Setup(v => v.Validate(request))
            .Returns(new ValidationResult
            {
                IsValid = false,
                Errors = new List<string> { "RecipeId is required", "ElementsPerBag must be non-negative" }
            });

        // Act
        var result = await _useCase.ExecuteAsync("CM01_1", request);

        // Assert
        Assert.IsFalse(result);
        _mockRepository.Verify(r => r.ApplyConfigurationAsync(It.IsAny<string>(), It.IsAny<ConfigurationRequest>()), Times.Never);
    }
}
