using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateWorkOrderDtoValidator : AbstractValidator<CreateWorkOrderDto>
{
    public CreateWorkOrderDtoValidator()
    {
        RuleFor(x => x.EquipmentId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.ScheduledDate).GreaterThan(DateTime.UtcNow).WithMessage("ScheduledDate must be in the future");
        RuleFor(x => x.Priority).NotEmpty();
    }
}