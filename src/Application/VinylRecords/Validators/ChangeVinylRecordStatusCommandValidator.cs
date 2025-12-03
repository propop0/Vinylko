using FluentValidation;

namespace Application.VinylRecords.Commands;

public class ChangeVinylRecordStatusCommandValidator : AbstractValidator<ChangeVinylRecordStatusCommand>
{
    public ChangeVinylRecordStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Vinyl record ID is required.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status must be a valid VinylRecordStatus value.");
    }
}


