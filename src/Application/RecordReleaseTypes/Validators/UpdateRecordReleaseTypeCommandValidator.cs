using FluentValidation;

namespace Application.RecordReleaseTypes.Validators;

public class UpdateRecordReleaseTypeCommandValidator : AbstractValidator<Commands.UpdateRecordReleaseTypeCommand>
{
    public UpdateRecordReleaseTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be a valid ReleaseType value.");
    }
}

