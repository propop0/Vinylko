using System;
using FluentValidation;

namespace Application.Sales.Commands
{
    public class CompleteSaleCommandValidator : AbstractValidator<CompleteSaleCommand>
    {
        public CompleteSaleCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);

            RuleFor(x => x.Notes)
                .MaximumLength(1000);
        }
    }
}


