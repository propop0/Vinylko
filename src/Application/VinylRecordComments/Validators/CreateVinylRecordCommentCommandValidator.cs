using FluentValidation;

namespace Application.VinylRecordComments.Commands;

public class CreateVinylRecordCommentCommandValidator : AbstractValidator<CreateVinylRecordCommentCommand>
{
    public CreateVinylRecordCommentCommandValidator()
    {
        RuleFor(x => x.VinylRecordId)
            .NotEqual(Guid.Empty)
            .WithMessage("Vinyl record ID is required.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Comment content is required.")
            .MaximumLength(2000)
            .WithMessage("Comment content must not exceed 2000 characters.");
    }
}


