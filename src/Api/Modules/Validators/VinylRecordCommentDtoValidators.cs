using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CreateVinylRecordCommentDtoValidator : AbstractValidator<CreateVinylRecordCommentDto>
{
    public CreateVinylRecordCommentDtoValidator()
    {
        RuleFor(x => x.Content).NotEmpty().WithMessage("Comment content cannot be empty.");
    }
}

public class UpdateVinylRecordCommentDtoValidator : AbstractValidator<UpdateVinylRecordCommentDto>
{
    public UpdateVinylRecordCommentDtoValidator()
    {
        RuleFor(x => x.Content).NotEmpty().WithMessage("Comment content cannot be empty.");
    }
}


