using FluentValidation;
using OLT.Core.CommandBus.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
