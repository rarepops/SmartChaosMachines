namespace LineControl.Domain.Entities;

public class CountingMachineDataChangedEventArgs : EventArgs
{
    public CountingMachineData Data { get; }

    public CountingMachineDataChangedEventArgs(CountingMachineData data)
    {
        Data = data;
    }
}
