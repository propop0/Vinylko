using FluentValidation;

namespace Application.Artists.Commands
{
    public class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
    {
        public CreateArtistCommandValidator()
        {
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


