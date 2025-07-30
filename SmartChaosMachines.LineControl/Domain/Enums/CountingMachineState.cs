namespace LineControl.Domain.Enums;

public enum CountingMachineState
{
    Disconnected,
    Connecting,
    Connected,
    Running,
    Stopped,
    Error,
    Unknown
}