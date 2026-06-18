using FluentValidation;
using Library.API.DTOs;

namespace Library.API.Validators;

public class BookValidator : AbstractValidator<BookDto>
{
    public BookValidator()
    {
        RuleFor(p => p.Title)
        .NotNull().WithMessage("Title is null")
        .NotEmpty().WithMessage("TItle is empty");

        RuleFor(p => p.Author)
        .NotNull().WithMessage("Author is null")
        .NotEmpty().WithMessage("Author is empty");
        
        RuleFor(p => p.Description)
        .NotNull().WithMessage("Description is null")
        .NotEmpty().WithMessage("Description is empty");
    }
}