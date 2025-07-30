using LineControl.Api.Contracts.Responses;
using LineControl.Application.Services;
using LineControl.Domain.Interfaces;

namespace LineControl.Application.UseCases.GetMachineData;

public class GetMachineDataUseCase
{
    private readonly IMachineRepository _repository;
    private readonly MachineDataProcessor _processor;

    public GetMachineDataUseCase(IMachineRepository repository, MachineDataProcessor processor)
    {
        _repository = repository;
        _processor = processor;
    }

    public async Task<GetMachineDataResponse?> ExecuteAsync(string position)
    {
        var data = await _repository.GetMachineDataAsync(position);
        return data is not null ? _processor.ProcessMachineData(data) : null;
    }
}
