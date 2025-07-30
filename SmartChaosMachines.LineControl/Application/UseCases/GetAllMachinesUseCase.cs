using LineControl.Api.Contracts.Responses;
using LineControl.Application.Services;
using LineControl.Domain.Interfaces;

namespace LineControl.Application.UseCases.GetAllMachines;

public class GetAllMachinesUseCase
{
    private readonly IMachineRepository _repository;
    private readonly MachineDataProcessor _processor;

    public GetAllMachinesUseCase(IMachineRepository repository, MachineDataProcessor processor)
    {
        _repository = repository;
        _processor = processor;
    }

    public async Task<IEnumerable<MachineStatusSummary>> ExecuteAsync()
    {
        var allData = await _repository.GetAllMachineDataAsync();
        return allData.Select(data => new MachineStatusSummary
        {
            Position = data.Position,
            Status = _processor.GetStatus(data),
            HasError = _processor.HasError(data),
            CurrentCount = _processor.GetCurrentCount(data),
            LastUpdate = data.Timestamp
        });
    }
}
