using LineControl.Domain.Entities;

namespace LineControl.Domain.Interfaces;

public interface ICountingMachineFactory
{
    ICountingMachine CreateMachine(string position);
    ICountingMachine CreateCountingMachine(CountingMachineEndpoint endpoint);
}
