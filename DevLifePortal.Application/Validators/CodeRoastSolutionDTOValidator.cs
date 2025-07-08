using DevLifePortal.Application.DTOs;
using FluentValidation;

namespace DevLifePortal.Application.Validators
{
    public class CodeRoastSolutionDTOValidator : AbstractValidator<CodeRoastSolutionDTO>
    {
        public CodeRoastSolutionDTOValidator()
        {
            RuleFor(u => u.ProblemName)
                .NotEmpty().WithMessage("Problem name is required.");

            RuleFor(u => u.Solution)
                .NotEmpty().WithMessage("Solution is required.");
        }
    }
}
