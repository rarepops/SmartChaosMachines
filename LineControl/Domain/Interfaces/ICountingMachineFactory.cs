using LineControl.Domain.Entities;

namespace LineControl.Domain.Interfaces;

public interface ICountingMachineFactory
{
    ICountingMachine CreateCountingMachine(CountingMachineEndpoint endpoint);
}