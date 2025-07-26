using LineControl.Application.Services;
using LineControl.Domain.Entities;
using LineControl.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LineControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountingMachinesController(
    LineControlService lineControlService,
    ILogger<CountingMachinesController> logger) : ControllerBase
{
    private readonly LineControlService _lineControlService = lineControlService;
    private readonly ILogger<CountingMachinesController> _logger = logger;

    /// <summary>
    /// Get current data from a specific counting machine
    /// </summary>
    /// <param name="position">Machine position (e.g., CM001, CM002)</param>
    /// <returns>Current machine data including count, status, and performance metrics</returns>
    [HttpGet("{position}")]
    public async Task<ActionResult<CountingMachineData>> GetMachineData(string position)
    {
        try
        {
            var data = await _lineControlService.GetMachineDataAsync(position);
            if (data == null)
            {
                _logger.LogWarning("Machine {Position} not found or not connected", position);
                return NotFound($"Machine {position} not found");
            }

            _logger.LogDebug("Retrieved data for machine {Position}: Count={Count}, Status={Status}",
                position, data.CurrentCount, data.Status);

            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting data for machine {Position}", position);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Apply configuration to a counting machine (simulates order changeover)
    /// </summary>
    /// <param name="position">Machine position to configure</param>
    /// <param name="request">Configuration parameters for the new recipe</param>
    /// <returns>Success/failure status of configuration application</returns>
    [HttpPost("{position}/configure")]
    public async Task<ActionResult> ConfigureMachine(string position, [FromBody] ConfigurationRequest request)
    {
        try
        {
            if (!request.IsValid)
            {
                _logger.LogWarning("Invalid configuration request for machine {Position}: {Request}", position, request);
                return BadRequest("Invalid configuration parameters");
            }

            _logger.LogInformation("Applying configuration to machine {Position}: Recipe={RecipeId}, ElementsPerBag={ElementsPerBag}",
                position, request.RecipeId, request.ElementsPerBag);

            var config = new CountingMachineConfig(
                position,
                new Dictionary<string, TagValue>
                {
                    ["RecipeId"] = new TagValue(request.RecipeId, TagType.String),
                    ["ElementsPerBag"] = new TagValue(request.ElementsPerBag, TagType.Int),
                    ["CountingSpeed"] = new TagValue(request.CountingSpeed, TagType.Int),
                    ["BeltSpeed"] = new TagValue(request.FeederSpeed, TagType.Int),
                    ["VibrationStrength"] = new TagValue(request.VibrationStrength, TagType.Int)
                }
            );

            var success = await _lineControlService.ApplyConfigurationAsync(position, config);

            if (success)
            {
                _logger.LogInformation("Configuration applied successfully to machine {Position}", position);
                return Ok(new
                {
                    Message = $"Configuration applied to machine {position}",
                    request.RecipeId,
                    Timestamp = DateTime.UtcNow
                });
            }

            _logger.LogWarning("Failed to apply configuration to machine {Position}", position);
            return BadRequest($"Failed to apply configuration to machine {position}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error configuring machine {Position}", position);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get status overview of all counting machines on the line
    /// </summary>
    /// <returns>List of machine status summaries</returns>
    [HttpGet]
    public async Task<ActionResult<List<MachineStatusSummary>>> GetAllMachines()
    {
        try
        {
            var machines = await _lineControlService.GetAllMachinesStatusAsync();
            return Ok(machines);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all machine statuses");
            return StatusCode(500, "Internal server error");
        }
    }
}

// DTOs for API
public record ConfigurationRequest(
    string RecipeId,
    int ElementsPerBag,
    int CountingSpeed,
    int FeederSpeed,
    int VibrationStrength
)
{
    /// <summary>
    /// Validates that all configuration parameters are within acceptable ranges
    /// </summary>
    public bool IsValid =>
        !string.IsNullOrWhiteSpace(RecipeId) &&
        ElementsPerBag > 0 &&
        CountingSpeed is >= 0 and <= 100 &&
        FeederSpeed is >= 0 and <= 100 &&
        VibrationStrength is >= 0 and <= 10;
}

public record MachineStatusSummary(
    string Position,
    string Status,
    int CurrentCount,
    string RecipeId,
    bool HasError
);
