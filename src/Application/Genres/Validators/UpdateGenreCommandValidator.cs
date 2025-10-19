using System;
using FluentValidation;

namespace Application.Genres.Commands
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}


