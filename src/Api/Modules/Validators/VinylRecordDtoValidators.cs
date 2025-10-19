using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateVinylRecordDtoValidator : AbstractValidator<CreateVinylRecordDto>
{
    public CreateVinylRecordDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(300)
            .WithMessage("Title is required and must not exceed 300 characters");

        RuleFor(x => x.Genre)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Genre is required and must not exceed 100 characters");

        RuleFor(x => x.ReleaseYear)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
            .WithMessage($"Release year must be between 1900 and {DateTime.UtcNow.Year + 1}");

        RuleFor(x => x.ArtistId)
            .NotEmpty()
            .WithMessage("Artist ID is required");

        RuleFor(x => x.LabelId)
            .NotEmpty()
            .WithMessage("Label ID is required");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Price must be greater than 0 and less than or equal to 999,999.99");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

public class UpdateVinylRecordDtoValidator : AbstractValidator<UpdateVinylRecordDto>
{
    public UpdateVinylRecordDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(300)
            .WithMessage("Title is required and must not exceed 300 characters");

        RuleFor(x => x.Genre)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Genre is required and must not exceed 100 characters");

        RuleFor(x => x.ReleaseYear)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
            .WithMessage($"Release year must be between 1900 and {DateTime.UtcNow.Year + 1}");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .LessThanOrEqualTo(999999.99m)
            .WithMessage("Price must be greater than 0 and less than or equal to 999,999.99");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

public class ChangeVinylRecordStatusDtoValidator : AbstractValidator<ChangeVinylRecordStatusDto>
{
    public ChangeVinylRecordStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(BeValidStatus)
            .WithMessage("Status must be one of: InStock, Reserved, Sold");
    }

    private static bool BeValidStatus(string status)
    {
        return Enum.TryParse<Domain.VinylRecords.VinylRecordStatus>(status, true, out _);
    }
}
