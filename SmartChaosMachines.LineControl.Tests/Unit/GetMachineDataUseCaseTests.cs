using LineControl.Application.Services;
using LineControl.Application.UseCases.GetMachineData;
using LineControl.Domain.Entities;
using LineControl.Domain.Enums;
using LineControl.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace SmartChaosMachines.LineControl.Tests.Unit;

[TestClass]
public class GetMachineDataUseCaseTests
{
    private Mock<IMachineRepository> _mockRepository;
    private MachineDataProcessor _processor;
    private GetMachineDataUseCase _useCase;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IMachineRepository>();
        _processor = new MachineDataProcessor();
        _useCase = new GetMachineDataUseCase(_mockRepository.Object, _processor);
    }

    [TestMethod]
    public async Task ExecuteAsync_MachineExists_ReturnsData()
    {
        // Arrange
        var machineData = new CountingMachineData("CM01_1", DateTime.UtcNow, new Dictionary<string, TagValue>
        {
            ["RecipeId"] = new TagValue("TEST001", TagType.String),
            ["CurrentCount"] = new TagValue(25, TagType.Int),
            ["Status"] = new TagValue("Running", TagType.String),
            ["ElementsPerBag"] = new TagValue(24, TagType.Int),
            ["CountingSpeed"] = new TagValue(50, TagType.Int),
            ["FeederSpeed"] = new TagValue(75, TagType.Int),
            ["VibrationStrength"] = new TagValue(3, TagType.Int),
            ["IsUsedInOrder"] = new TagValue(true, TagType.Bool),
            ["HasError"] = new TagValue(false, TagType.Bool)
        });

        _mockRepository.Setup(r => r.GetMachineDataAsync("CM01_1"))
            .ReturnsAsync(machineData);

        // Act
        var result = await _useCase.ExecuteAsync("CM01_1");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("CM01_1", result.Position);
        Assert.AreEqual("TEST001", result.RecipeId);
        Assert.AreEqual(25, result.CurrentCount);
        Assert.AreEqual("Running", result.Status);
    }

    [TestMethod]
    public async Task ExecuteAsync_MachineNotFound_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetMachineDataAsync("INVALID"))
            .ReturnsAsync((CountingMachineData?)null);

        // Act
        var result = await _useCase.ExecuteAsync("INVALID");

        // Assert
        Assert.IsNull(result);
    }
}
