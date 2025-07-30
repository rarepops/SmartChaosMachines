namespace LineControl.Domain.Interfaces;

public interface IMachineManager
{
    Task<ICountingMachine> GetOrCreateMachineAsync(string position);
    Task DisconnectAllAsync();
}
