using FluentValidation;

namespace Application.RecordReleaseTypes.Validators;

public class CreateRecordReleaseTypeCommandValidator : AbstractValidator<Commands.CreateRecordReleaseTypeCommand>
{
    public CreateRecordReleaseTypeCommandValidator()
    {
        RuleFor(x => x.VinylRecordId)
            .NotEmpty()
            .WithMessage("VinylRecordId is required.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be a valid ReleaseType value.");
    }
}

