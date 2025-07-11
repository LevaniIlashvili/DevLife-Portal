using DevLifePortal.Application.DTOs;
using FluentValidation;

namespace DevLifePortal.Application.Validators
{
    public class ExcuseGeneratorGeneratoExcuseDTOValidator : AbstractValidator<ExcuseGeneratorGenerateExcuseDTO>
    {
        public ExcuseGeneratorGeneratoExcuseDTOValidator()
        {
            RuleFor(e => e.Category).NotEmpty();

            RuleFor(e => e.Type).IsInEnum();
        }
    }
}
