using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateMaintenanceScheduleDtoValidator : AbstractValidator<CreateMaintenanceScheduleDto>
{
    public CreateMaintenanceScheduleDtoValidator()
    {
        RuleFor(x => x.EquipmentId).NotEmpty();
        RuleFor(x => x.TaskName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.NextDueDate).GreaterThan(DateTime.UtcNow).WithMessage("NextDueDate must be in the future");
        RuleFor(x => x.Frequency).NotEmpty(); 
    }
}