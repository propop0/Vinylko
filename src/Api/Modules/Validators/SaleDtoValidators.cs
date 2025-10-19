using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateSaleDtoValidator : AbstractValidator<CreateSaleDto>
{
    public CreateSaleDtoValidator()
    {
        RuleFor(x => x.RecordId)
            .NotEmpty()
            .WithMessage("Record ID is required");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Price must be greater than 0 and less than or equal to 999,999.99");

        RuleFor(x => x.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale date cannot be in the future");

        RuleFor(x => x.CustomerName)
            .MaximumLength(200)
            .WithMessage("Customer name must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.CustomerName));

        RuleFor(x => x.CustomerEmail)
            .EmailAddress()
            .WithMessage("Customer email must be a valid email address")
            .When(x => !string.IsNullOrEmpty(x.CustomerEmail));
    }
}

public class CompleteSaleDtoValidator : AbstractValidator<CompleteSaleDto>
{
    public CompleteSaleDtoValidator()
    {
        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

public class UpdateSaleCustomerDtoValidator : AbstractValidator<UpdateSaleCustomerDto>
{
    public UpdateSaleCustomerDtoValidator()
    {
        RuleFor(x => x.CustomerName)
            .MaximumLength(200)
            .WithMessage("Customer name must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.CustomerName));

        RuleFor(x => x.CustomerEmail)
            .EmailAddress()
            .WithMessage("Customer email must be a valid email address")
            .When(x => !string.IsNullOrEmpty(x.CustomerEmail));
    }
}
