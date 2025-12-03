using FluentValidation;

namespace Application.VinylRecordComments.Commands;

public class UpdateVinylRecordCommentCommandValidator : AbstractValidator<UpdateVinylRecordCommentCommand>
{
    public UpdateVinylRecordCommentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Comment ID is required.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Comment content is required.")
            .MaximumLength(2000)
            .WithMessage("Comment content must not exceed 2000 characters.");
    }
}


