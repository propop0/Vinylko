using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateEquipmentDtoValidator : AbstractValidator<CreateEquipmentDto>
{
    public CreateEquipmentDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.InstallationDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Installation date cannot be in the future");
    }
}