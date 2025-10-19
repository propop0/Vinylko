using System;
using FluentValidation;

namespace Application.Artists.Commands
{
    public class UpdateArtistCommandValidator : AbstractValidator<UpdateArtistCommand>
    {
        public UpdateArtistCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Bio)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}


