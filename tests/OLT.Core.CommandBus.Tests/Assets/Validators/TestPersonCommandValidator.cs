using FluentValidation;
using OLT.Core.CommandBus.Tests.Assets.Models;

namespace OLT.Core.CommandBus.Tests.Assets.Validators
{
    public class TestPersonCommandValidator : AbstractValidator<TestCommandDtoHelper>
    {
        public TestPersonCommandValidator()
        {
            RuleFor(p => p.Value).GreaterThan(0).WithMessage("Value Required");
        }
    }
}
