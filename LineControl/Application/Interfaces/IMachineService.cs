using LineControl.Api.Contracts;
using LineControl.Api.Contracts.Responses;

namespace LineControl.Application.Interfaces;

public interface IMachineService
{
    Task<GetMachineDataResponse?> GetMachineDataAsync(string position);
    Task<IEnumerable<MachineStatusSummary>> GetAllMachinesAsync();
    Task<bool> ConfigureMachineAsync(string position, ConfigurationRequest request);
}
