using FluentValidation;
using OLT.Core.CommandBus.Tests.Assets.Models;

namespace OLT.Core.CommandBus.Tests.Assets.Validators
{
    public class TestPersonDtoValidator : AbstractValidator<TestPersonDto>
    {
        public TestPersonDtoValidator()
        {            
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("ID Required");
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("First Name Required");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("Last Name Required");
        }
    }
}
