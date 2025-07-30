using LineControl.Api.Contracts;
using LineControl.Domain.Entities;
using LineControl.Domain.Enums;
using LineControl.Domain.Interfaces;

namespace LineControl.Infrastructure.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly IMachineManager _machineManager;
    private readonly IMachinePositionConfiguration _positionConfig;
    private readonly ILogger<MachineRepository> _logger;

    public MachineRepository(
        IMachineManager machineManager,
        IMachinePositionConfiguration positionConfig,
        ILogger<MachineRepository> logger)
    {
        _machineManager = machineManager;
        _positionConfig = positionConfig;
        _logger = logger;
    }

    public async Task<CountingMachineData?> GetMachineDataAsync(string position)
    {
        try
        {
            var machine = await _machineManager.GetOrCreateMachineAsync(position);
            var data = await machine.ReadTagsAsync();
            return new CountingMachineData(position, DateTime.UtcNow, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading data for machine at position {Position}", position);
            return null;
        }
    }

    public async Task<IEnumerable<CountingMachineData>> GetAllMachineDataAsync()
    {
        var positions = _positionConfig.GetAllPositions();
        var tasks = positions.Select(GetMachineDataAsync);
        var results = await Task.WhenAll(tasks);
        return results.Where(data => data != null)!;
    }

    public async Task<bool> ApplyConfigurationAsync(string position, ConfigurationRequest request)
    {
        try
        {
            var machine = await _machineManager.GetOrCreateMachineAsync(position);

            var configTags = new Dictionary<string, TagValue>
            {
                ["RecipeId"] = new TagValue(request.RecipeId, TagType.String),
                ["ElementsPerBag"] = new TagValue(request.ElementsPerBag, TagType.Int),
                ["CountingSpeed"] = new TagValue(request.CountingSpeed, TagType.Int),
                ["FeederSpeed"] = new TagValue(request.FeederSpeed, TagType.Int),
                ["VibrationStrength"] = new TagValue(request.VibrationStrength, TagType.Int)
            };

            // This now updates the EXISTING machine instance
            await machine.WriteTagsAsync(configTags);

            _logger.LogInformation("Successfully applied configuration to machine at position {Position}", position);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error configuring machine at position {Position}", position);
            return false;
        }
    }

    public async Task<CountingMachineConfig?> GetMachineConfigAsync(string position)
    {
        try
        {
            var machine = await _machineManager.GetOrCreateMachineAsync(position);
            var configTags = await machine.ReadTagsAsync();
            return new CountingMachineConfig(position, configTags);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading config for machine at position {Position}", position);
            return null;
        }
    }
}
