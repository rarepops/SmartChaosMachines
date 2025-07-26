using LineControl.Domain.Entities;
using LineControl.Domain.Enums;

namespace LineControl.Domain.Interfaces;

public interface ICountingMachine
{
    CountingMachineEndpoint Endpoint { get; }
    CountingMachineState State { get; }

    Task<bool> ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync();

    // We will use this to poll the machine data every X seconds
    Task<CountingMachineData> ReadDataAsync();

    // We will use this to write our configuration to the machine on changeover
    Task WriteConfigurationAsync(CountingMachineConfig config);

    Task<T> ReadTagAsync<T>(string tagName);
    Task WriteTagAsync<T>(string tagName, T value);

    event EventHandler<CountingMachineDataChangedEventArgs> DataChanged;
}