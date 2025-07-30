using LineControl.Api.Contracts;
using LineControl.Domain.Interfaces;

namespace LineControl.Application.UseCases.ConfigureMachine;

public class ConfigureMachineUseCase
{
    private readonly IMachineRepository _repository;
    private readonly IConfigurationValidator _validator;

    public ConfigureMachineUseCase(IMachineRepository repository, IConfigurationValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<bool> ExecuteAsync(string position, ConfigurationRequest request)
    {
        var validation = _validator.Validate(request);
        if (!validation.IsValid) return false;

        return await _repository.ApplyConfigurationAsync(position, request);
    }
}
