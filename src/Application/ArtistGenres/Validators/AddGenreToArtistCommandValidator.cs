using FluentValidation;

namespace Application.ArtistGenres.Validators;

public class AddGenreToArtistCommandValidator : AbstractValidator<Commands.AddGenreToArtistCommand>
{
    public AddGenreToArtistCommandValidator()
    {
        RuleFor(x => x.ArtistId)
            .NotEmpty()
            .WithMessage("ArtistId is required.");

        RuleFor(x => x.GenreId)
            .NotEmpty()
            .WithMessage("GenreId is required.");
    }
}

