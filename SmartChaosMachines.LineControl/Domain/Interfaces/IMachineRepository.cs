using LineControl.Api.Contracts;
using LineControl.Domain.Entities;

namespace LineControl.Domain.Interfaces;

public interface IMachineRepository
{
    Task<CountingMachineData?> GetMachineDataAsync(string position);
    Task<IEnumerable<CountingMachineData>> GetAllMachineDataAsync();
    Task<bool> ApplyConfigurationAsync(string position, ConfigurationRequest request);
    Task<CountingMachineConfig?> GetMachineConfigAsync(string position);
}
