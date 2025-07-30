using LineControl.Application.Services;
using LineControl.Domain.Entities;
using LineControl.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartChaosMachines.LineControl.Tests.Unit;

[TestClass]
public class MachineDataProcessorTests
{
    private MachineDataProcessor _processor;

    [TestInitialize]
    public void Setup()
    {
        _processor = new MachineDataProcessor();
    }

    [TestMethod]
    public void ProcessMachineData_ValidData_ReturnsCorrectResponse()
    {
        // Arrange
        var data = new CountingMachineData("CM01_1", DateTime.UtcNow, new Dictionary<string, TagValue>
        {
            ["RecipeId"] = new TagValue("TEST001", TagType.String),
            ["CurrentCount"] = new TagValue(42, TagType.Int),
            ["Status"] = new TagValue("Running", TagType.String),
            ["ElementsPerBag"] = new TagValue(24, TagType.Int),
            ["CountingSpeed"] = new TagValue(75, TagType.Int),
            ["FeederSpeed"] = new TagValue(50, TagType.Int),
            ["VibrationStrength"] = new TagValue(3, TagType.Int),
            ["IsUsedInOrder"] = new TagValue(true, TagType.Bool),
            ["HasError"] = new TagValue(false, TagType.Bool)
        });

        // Act
        var result = _processor.ProcessMachineData(data);

        // Assert
        Assert.AreEqual("CM01_1", result.Position);
        Assert.AreEqual("TEST001", result.RecipeId);
        Assert.AreEqual(42, result.CurrentCount);
        Assert.AreEqual("Running", result.Status);
        Assert.AreEqual(24, result.ElementsPerBag);
        Assert.AreEqual(75, result.CountingSpeed);
        Assert.AreEqual(50, result.FeederSpeed);
        Assert.AreEqual(3, result.VibrationStrength);
        Assert.IsTrue(result.IsUsedInOrder);
        Assert.IsFalse(result.HasError);
    }

    [TestMethod]
    public void GetRecipeId_ValidTag_ReturnsValue()
    {
        // Arrange
        var data = new CountingMachineData("CM01_1", DateTime.UtcNow, new Dictionary<string, TagValue>
        {
            ["RecipeId"] = new TagValue("RECIPE123", TagType.String)
        });

        // Act
        var result = _processor.GetRecipeId(data);

        // Assert
        Assert.AreEqual("RECIPE123", result);
    }

    [TestMethod]
    public void GetCurrentCount_ValidTag_ReturnsValue()
    {
        // Arrange
        var data = new CountingMachineData("CM01_1", DateTime.UtcNow, new Dictionary<string, TagValue>
        {
            ["CurrentCount"] = new TagValue(100, TagType.Int)
        });

        // Act
        var result = _processor.GetCurrentCount(data);

        // Assert
        Assert.AreEqual(100, result);
    }
}
