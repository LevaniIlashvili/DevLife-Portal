using DevLifePortal.Application.DTOs;
using FluentValidation;

namespace DevLifePortal.Application.Validators
{
    public class DevDatingSwipeActionDTOValidator : AbstractValidator<DevDatingSwipeActionDTO>
    {
        public DevDatingSwipeActionDTOValidator()
        {
            RuleFor(x => x.TargetProfileId)
                .NotEmpty()
                .WithMessage("TargetProfileId must not be empty");

            RuleFor(x => x.Direction)
                .NotEmpty()
                .WithMessage("Swipe direction is required")
                .Must(d => d.Equals("left", StringComparison.OrdinalIgnoreCase) || d.Equals("right", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Direction must be either 'left' or 'right'");
        }
    }
}
