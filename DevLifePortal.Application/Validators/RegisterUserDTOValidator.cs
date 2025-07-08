using DevLifePortal.Application.DTOs;
using FluentValidation;

namespace DevLifePortal.Application.Validators
{
    public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserDTOValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(u => u.DateOfBirth)
                .LessThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Date of birth must be in the past.");

            RuleFor(u => u.TechStack)
                .NotEmpty().WithMessage("Tech stack is required.");

            RuleFor(u => u.ExperienceLevel)
                .NotEmpty().WithMessage("Experience level is required.");
        }
    }

}
