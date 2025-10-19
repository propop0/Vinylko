using System;
using FluentValidation;

namespace Application.Sales.Commands
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.RecordId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.SaleDate)
                .LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}


