using FluentValidation;
using Library.API.DTOs;

namespace Library.API.Validators;

public class ReservationValidator : AbstractValidator<ReservationDto>
{
    public ReservationValidator()
    {
        RuleFor(p => p.ClientId)
        .NotNull().WithMessage("Name is null")
        .NotEmpty().WithMessage("Name is empty");

        RuleFor(p => p.Book)
        .NotNull().WithMessage("Books is null")
        .NotEmpty().WithMessage("Books is empty");
    }
}