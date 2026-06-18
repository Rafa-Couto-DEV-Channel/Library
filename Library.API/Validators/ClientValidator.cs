using FluentValidation;
using Library.API.DTOs;

namespace Library.API.Validators;

public class ClientValidator : AbstractValidator<ClientDto>
{
    public ClientValidator()
    {
        RuleFor(p => p.Name)
        .NotNull().WithMessage("Name is null")
        .NotEmpty().WithMessage("Name is empty");

        RuleFor(p => p.Email)
        .NotNull().WithMessage("Email is null")
        .NotEmpty().WithMessage("Email is empty")
        .EmailAddress();
    }
}