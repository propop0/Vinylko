using System;
using FluentValidation;

namespace Application.VinylRecords.Commands
{
    public class CreateVinylRecordCommandValidator : AbstractValidator<CreateVinylRecordCommand>
    {
        public CreateVinylRecordCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Genre)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.ArtistId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.LabelId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.ReleaseYear)
                .InclusiveBetween(1900, DateTime.UtcNow.Year);

            RuleFor(x => x.Price)
                .GreaterThan(0);
        }
    }
}


