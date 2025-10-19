using System;
using FluentValidation;

namespace Application.Sales.Commands
{
    public class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
    {
        public CancelSaleCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }
}


