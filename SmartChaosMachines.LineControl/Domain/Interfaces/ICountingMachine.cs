using LineControl.Domain.Entities;

namespace LineControl.Domain.Interfaces;

public interface ICountingMachine : IDisposable
{
    Task<Dictionary<string, TagValue>> ReadTagsAsync();
    Task WriteTagsAsync(Dictionary<string, TagValue> tags);
}
