using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateArtistDtoValidator : AbstractValidator<CreateArtistDto>
{
    public CreateArtistDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Artist name is required and must not exceed 200 characters");

        RuleFor(x => x.Bio)
            .NotEmpty()
            .MaximumLength(2000)
            .WithMessage("Artist bio is required and must not exceed 2000 characters");

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Country is required and must not exceed 100 characters");

        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Birth date cannot be in the future")
            .When(x => x.BirthDate.HasValue);

        RuleFor(x => x.Website)
            .Must(BeValidUrl)
            .WithMessage("Website must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.Website));
    }

    private static bool BeValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}

public class UpdateArtistDtoValidator : AbstractValidator<UpdateArtistDto>
{
    public UpdateArtistDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Artist name is required and must not exceed 200 characters");

        RuleFor(x => x.Bio)
            .NotEmpty()
            .MaximumLength(2000)
            .WithMessage("Artist bio is required and must not exceed 2000 characters");

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Country is required and must not exceed 100 characters");

        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Birth date cannot be in the future")
            .When(x => x.BirthDate.HasValue);

        RuleFor(x => x.Website)
            .Must(BeValidUrl)
            .WithMessage("Website must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.Website));
    }

    private static bool BeValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}
