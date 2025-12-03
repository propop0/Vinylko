using FluentValidation;

namespace Application.Sales.Commands;

public class UpdateSaleCustomerCommandValidator : AbstractValidator<UpdateSaleCustomerCommand>
{
    public UpdateSaleCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Sale ID is required.");

        RuleFor(x => x.CustomerName)
            .MaximumLength(200)
            .WithMessage("Customer name must not exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.CustomerName));

        RuleFor(x => x.CustomerEmail)
            .EmailAddress()
            .WithMessage("Customer email must be a valid email address.")
            .MaximumLength(300)
            .WithMessage("Customer email must not exceed 300 characters.")
            .When(x => !string.IsNullOrEmpty(x.CustomerEmail));
    }
}


