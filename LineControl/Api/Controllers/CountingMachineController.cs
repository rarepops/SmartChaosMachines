using LineControl.Api.Contracts;
using LineControl.Api.Contracts.Responses;
using LineControl.Application.UseCases.ConfigureMachine;
using LineControl.Application.UseCases.GetAllMachines;
using LineControl.Application.UseCases.GetMachineData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LineControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CountingMachinesController : ControllerBase
{
    private readonly GetMachineDataUseCase _getMachineData;
    private readonly ConfigureMachineUseCase _configureMachine;
    private readonly GetAllMachinesUseCase _getAllMachines;
    private readonly ILogger<CountingMachinesController> _logger;

    public CountingMachinesController(
        GetMachineDataUseCase getMachineData,
        ConfigureMachineUseCase configureMachine,
        GetAllMachinesUseCase getAllMachines,
        ILogger<CountingMachinesController> logger)
    {
        _getMachineData = getMachineData;
        _configureMachine = configureMachine;
        _getAllMachines = getAllMachines;
        _logger = logger;
    }

    /// <summary>
    /// Gets detailed data for a specific counting machine by position.
    /// </summary>
    /// <param name="position">The machine position identifier (e.g., CM01_1, CM01_2, CM02_1, CM02_2)</param>
    /// <returns>Detailed machine data including status, counts, and configuration</returns>
    [HttpGet("{position}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetMachineDataResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetMachineDataResponse>> GetMachineData(string position)
    {
        var result = await _getMachineData.ExecuteAsync(position);
        if (result == null)
        {
            return NotFound($"Machine at position {position} not found or not accessible");
        }
        return Ok(result);
    }

    /// <summary>
    /// Gets a summary of all counting machines in the factory.
    /// </summary>
    /// <returns>List of machine status summaries for all positions</returns>
    [HttpGet("")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<MachineStatusSummary>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MachineStatusSummary>>> GetAllMachines()
    {
        var machines = await _getAllMachines.ExecuteAsync();
        return Ok(machines);
    }

    /// <summary>
    /// Configures a specific counting machine with new settings.
    /// </summary>
    /// <param name="position">The machine position identifier (e.g., CM01_1, CM01_2, CM02_1, CM02_2)</param>
    /// <param name="request">Configuration parameters including recipe, speeds, and vibration settings</param>
    /// <returns>Success or failure indication</returns>
    [HttpPost("{position}/configure")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConfigureMachine(string position, [FromBody] ConfigurationRequest request)
    {
        var success = await _configureMachine.ExecuteAsync(position, request);
        if (!success)
        {
            return BadRequest("Configuration failed - invalid parameters or machine not accessible");
        }
        return Ok();
    }
}
